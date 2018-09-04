using SQLite4Unity3d;

public class StreetCrossingData
{

    //the classes in SQLITE ADDING AND RECORDING and in the same name and type as in the sqlite db tables, you can check streamingAssets/USN_Simulation.db to see the hierarchy

    [PrimaryKey, AutoIncrement]
    public int street_crossing_data_id { get; set; }

    public int gameplay_id { get; set; }
    [NotNull]

    public string traffic_flow_towards { get; set; }
    public double current_time_span { get; set; }

    public double current_distance_nearest_car { get; set; }

    public bool gazing_car { get; set; }
    public bool gazing_nearest_car { get; set; }

    public bool after_collision_frame { get; set; }
    public double person_x { get; set; }
    public double person_y { get; set; }
    public double person_z { get; set; }
    public double head_rotation_y { get; set; }


}
