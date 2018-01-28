using SQLite4Unity3d;

public class TrackingInfo   {

		[PrimaryKey,AutoIncrement]
	    public int tracking_id {get;set;}
		[NotNull]
	    public int player_id {get;set;}
		[NotNull]

	    public int enviroment_id {get;set;}

	    public int timespan {get;set;}
		
	    public int distance_neares {get;set;}

	    public int gazing_x {get;set;}

	    public int gazing_y {get;set;}

	    public int gazing_z {get;set;}

	    public int gaze_nearest_car {get;set;}

	    public int after_collision_frame {get;set;}


	 
}
