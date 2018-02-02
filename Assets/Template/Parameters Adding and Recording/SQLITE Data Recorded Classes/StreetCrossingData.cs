using SQLite4Unity3d;

public class StreetCrossingData   {

		[PrimaryKey,AutoIncrement]
	    public int street_crossing_data_id {get;set;}

	    public int gameplay_id {get;set;}
		[NotNull]

		public string traffic_flow_towards {get;set;}
	    public int current_time_span {get;set;}
		
	    public double current_distance_nearest_car {get;set;}

	    public bool gaze_car {get;set;}
	    public bool gaze_nearest_car {get;set;}

	    public bool after_collision_frame {get;set;}


	 
}
