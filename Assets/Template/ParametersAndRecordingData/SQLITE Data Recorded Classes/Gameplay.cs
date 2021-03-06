﻿using SQLite4Unity3d;
public class Gameplay
{

    //the classes in SQLITE ADDING AND RECORDING are in the same name and type as in the sqlite db tables, you can check streamingAssets/USN_Simulation.db to see the hierarchy
    [PrimaryKey, AutoIncrement]
    public int gameplay_id { get; set; }
    [NotNull]
    public string street_direction { get; set; }
    [NotNull]

    public int lanes_per_direction { get; set; }
    [NotNull]

    public int car_speed_km { get; set; }

    public int car_span_second { get; set; }
    [NotNull]

    public string sound_mode { get; set; }

    public string player_name { get; set; }
    public double player_height { get; set; }
    public int number_of_accidents_per_player { get; set; }
    public int number_of_successful_crossing_per_player { get; set; }
    public string crossing_type { get; set; }
    public System.DateTime date{get;set;}

}
