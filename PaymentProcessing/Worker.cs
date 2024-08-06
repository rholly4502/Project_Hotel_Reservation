using System.Reflection.Metadata;
using Microsoft.EntityFrameworkCore;
using PaymentProcessing.Models;

namespace PaymentProcessing
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IServiceProvider _serviceProvider;
        private int _lastProcessedPaymentId;

        public Worker(ILogger<Worker> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _lastProcessedPaymentId = 0;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker service is starting.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var dbContext = scope.ServiceProvider.GetRequiredService<HotelReservationSystemDBContext>();

                        // Check if we can connect to the database
                        if (await dbContext.Database.CanConnectAsync(stoppingToken))
                        {
                            _logger.LogInformation("Connected to the database successfully.");

                            var pendingPayments = dbContext.Payments
                                .Include(p => p.Reservation).ThenInclude(p => p.Customer)
                                .Include(p => p.Reservation).ThenInclude(p => p.Room)
                                .Where(p => p.PaymentId > _lastProcessedPaymentId)
                                .ToList();

                            foreach (var payment in pendingPayments)
                            {
                                DisplayReceiptInfo(payment);
                                _lastProcessedPaymentId = payment.PaymentId; // Update last processed payment ID
                            }

                            await dbContext.SaveChangesAsync(stoppingToken);
                        }
                        else
                        {
                            _logger.LogWarning("Unable to connect to the database.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while trying to connect to the database.");
                }

                // Check for new payments every minute
                await Task.Delay(TimeSpan.FromSeconds(3), stoppingToken);
            }
        }

        private void DisplayReceiptInfo(Payment payment)
        {
            var reservation = payment.Reservation;
            var customer = reservation.Customer;
            var room = reservation.Room;

            string receiptInfo = $"Receipt ID           : {payment.PaymentId}\n" +
                                 $"Reservation ID       : {payment.ReservationId}\n" +
                                 $"Customer Name        : {customer.Name}\n" +
                                 $"Room Number          : {room.RoomNumber}\n" +
                                 $"Room Type            : {room.RoomType}\n" +
                                 $"Check-In Date        : {reservation.CheckInDate}\n" +
                                 $"Check-Out Date       : {reservation.CheckOutDate}\n" +
                                 $"Amount Paid          : ${payment.Amount}\n" +
                                 $"Payment Date         : {payment.PaymentDate}\n" +
                                 "============================================\n" +
                                 "Thank you for your payment!";

            _logger.LogInformation("Receipt displayed: \n{receiptInfo}", receiptInfo);
        }
    }
}
