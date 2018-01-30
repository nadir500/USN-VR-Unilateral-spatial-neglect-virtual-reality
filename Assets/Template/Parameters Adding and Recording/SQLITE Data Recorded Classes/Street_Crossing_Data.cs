using SQLite4Unity3d;

public class Street_Crossing_Data   {

		[PrimaryKey,AutoIncrement]
	    public int street_crossing_data_id {get;set;}

	    public int player_id {get;set;}
		[NotNull]

		public string traffic_flow_towards {get;set;}
	    public int current_time_span {get;set;}
		
	    public float current_distance_nearest_car {get;set;}

	    public bool gaze_car {get;set;}
	    public bool gaze_nearest_car {get;set;}

	    public bool after_collision_frame {get;set;}


	 
}
