﻿namespace YogurtCleaning.Models
{
    public class CleaningObjectUpdateRequest
    {
        public int NumberOfRooms { get; set; }
        public int NumberOfBathrooms { get; set; }
        public int Square { get; set; }
        public int NumberOfWindows { get; set; }
        public int NumberOfBalconies { get; set; }
        public string Address { get; set; }
    }
}
