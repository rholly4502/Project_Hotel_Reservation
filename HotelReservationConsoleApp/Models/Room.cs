﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace HotelReservationConsoleApp.Models;

public partial class Room
{
    public int RoomId { get; set; }

    public int RoomNumber { get; set; }

    public string RoomType { get; set; }

    public decimal Price { get; set; }

    public string AvailabilityStatus { get; set; }

    public virtual ICollection<Reservation> Reservations { get; set; } = new List<Reservation>();
}