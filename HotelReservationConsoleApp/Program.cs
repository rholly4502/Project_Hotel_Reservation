// See https://aka.ms/new-console-template for more information
using System;
using HotelReservationConsoleApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

var serviceProvider = new ServiceCollection()
	.AddDbContext<HotelReservationSystemDBContext>(options => options.UseSqlServer("HotelReservationSystem"))
	.BuildServiceProvider();

var dbContext = serviceProvider.GetRequiredService<HotelReservationSystemDBContext>();

while (true)
{
	Console.WriteLine("Hotel Room Management System");
	Console.WriteLine("----------------------------");
	Console.WriteLine("1. Add Room");
	Console.WriteLine("2. View Room");
	Console.WriteLine("3. Update Room");
	Console.WriteLine("4. Delete Room");
	Console.WriteLine("5. Exit");
	Console.Write("Select an option: ");
	var option = Console.ReadLine();

	switch (option)
	{
		case "1":
			AddRoom(dbContext);
			break;
		case "2":
			ViewRoom(dbContext);
			break;
		case "3":
			UpdateRoom(dbContext);
			break;
		case "4":
			DeleteRoom(dbContext);
			break;
		case "5":
			return;
		default:
			Console.WriteLine("Invalid option. Please try again.");
			Pause();
			break;
	}
}

// Create
static void AddRoom(HotelReservationSystemDBContext dbContext)
{
	Console.WriteLine("--------");
	Console.WriteLine("Add Room");
	Console.WriteLine("--------");

	var room = new Room();

	Console.Write("Enter Room Number        : ");
	room.RoomNumber = Convert.ToInt32(Console.ReadLine());

	Console.Write("Enter Room Type          : ");
	room.RoomType = Console.ReadLine();

	Console.Write("Enter Price              : ");
	room.Price = Convert.ToDecimal(Console.ReadLine());

	room.AvailabilityStatus = "Available"; // Default value

	dbContext.Rooms.Add(room);
	dbContext.SaveChanges();

	if (room.RoomId > 0)
	{
		Console.WriteLine();
		Console.WriteLine($"Room {room.RoomNumber} added successfully:");
		Console.WriteLine($"{"Room ID              :".PadRight(25)} {room.RoomId}");
		Console.WriteLine($"{"Room Number          :".PadRight(25)} {room.RoomNumber}");
		Console.WriteLine($"{"Room Type            :".PadRight(25)} {room.RoomType}");
		Console.WriteLine($"{"Price                :".PadRight(25)} {room.Price}");
		Console.WriteLine($"{"Availability Status  :".PadRight(25)} {room.AvailabilityStatus}");
		Console.WriteLine();
	}
	else
	{
		Console.WriteLine("Failed to add room.");
	}
	Pause();
}

// Read
void ViewRoom(HotelReservationSystemDBContext dbContext)
{
	Console.WriteLine("---------");
	Console.WriteLine("View Room");
	Console.WriteLine("---------");

	Console.Write("Enter Room Number        : ");
	var roomNumber = Convert.ToInt32(Console.ReadLine());
	var room = dbContext.Rooms.SingleOrDefault(r => r.RoomNumber == roomNumber);

	if (room != null)
	{
		Console.WriteLine();
		Console.WriteLine($"Room {room.RoomNumber} Information");
		Console.WriteLine("---------------------------------------");
		Console.WriteLine($"{"Room ID              :".PadRight(25)} {room.RoomId}");
		Console.WriteLine($"{"Room Number          :".PadRight(25)} {room.RoomNumber}");
		Console.WriteLine($"{"Room Type            :".PadRight(25)} {room.RoomType}");
		Console.WriteLine($"{"Price                :".PadRight(25)} {room.Price}");
		Console.WriteLine($"{"Availability Status  :".PadRight(25)} {room.AvailabilityStatus}");
		Console.WriteLine();
	}
	else
	{
		Console.WriteLine("Room not found.");
	}
	Pause();
}

// Update
void UpdateRoom(HotelReservationSystemDBContext dbContext)
{
	Console.WriteLine("===========");
	Console.WriteLine("Update Room");
	Console.WriteLine("===========");

	Console.Write("Enter Room Number        : ");
	var roomNumber = Convert.ToInt32(Console.ReadLine());
	var room = dbContext.Rooms.SingleOrDefault(r => r.RoomNumber == roomNumber);

	if (room != null)
	{
		Console.WriteLine($"{"Room ID              :".PadRight(25)} {room.RoomId}");
		Console.WriteLine($"{"Room Number          :".PadRight(25)} {room.RoomNumber}");
		Console.WriteLine($"{"Room Type            :".PadRight(25)} {room.RoomType}");
		Console.WriteLine($"{"Price                :".PadRight(25)} {room.Price}");
		Console.WriteLine($"{"Availability Status  :".PadRight(25)} {room.AvailabilityStatus}");

		Console.Write("Enter Availability Status: ");
		room.AvailabilityStatus = Console.ReadLine();

		dbContext.Rooms.Update(room);
		dbContext.SaveChanges();

		Console.WriteLine();
		Console.WriteLine("Room updated successfully:");
		Console.WriteLine($"{"Room ID              :".PadRight(25)} {room.RoomId}");
		Console.WriteLine($"{"Room Number          :".PadRight(25)} {room.RoomNumber}");
		Console.WriteLine($"{"Room Type            :".PadRight(25)} {room.RoomType}");
		Console.WriteLine($"{"Price                :".PadRight(25)} {room.Price}");
		Console.WriteLine($"{"Availability Status  :".PadRight(25)} {room.AvailabilityStatus}");
	}
	else
	{
		Console.WriteLine("Room not found.");
	}
	Pause();
}

// Delete
void DeleteRoom(HotelReservationSystemDBContext dbContext)
{
	Console.WriteLine("===========");
	Console.WriteLine("Delete Room");
	Console.WriteLine("===========");

	Console.Write("Enter Room Number        : ");
	var roomNumber = Convert.ToInt32(Console.ReadLine());
	var room = dbContext.Rooms.SingleOrDefault(r => r.RoomNumber == roomNumber);

	if (room != null)
	{
		dbContext.Rooms.Remove(room);
		dbContext.SaveChanges();

		Console.WriteLine();
		Console.WriteLine("Room deleted successfully:");
		Console.WriteLine($"{"Room ID              :".PadRight(25)} {room.RoomId}");
		Console.WriteLine($"{"Room Number          :".PadRight(25)} {room.RoomNumber}");
	}
	else
	{
		Console.WriteLine("Room not found.");
	}
	Pause();
}

static void Pause()
{
	Console.WriteLine("Press any key to return to the main menu...");
	Console.ReadKey(true);
	Console.WriteLine();
}