﻿namespace UI.ApiLibrary.Dto;

public class WarningDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Province { get; set; }
    public string MushroomName { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public DateTime Date { get; set; }
    public bool IsActive { get; set; }
}
