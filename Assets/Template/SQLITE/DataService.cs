using SQLite4Unity3d;
using UnityEngine;
#if !UNITY_EDITOR
using System.Collections;
using System.IO;
#endif
using System.Collections.Generic;

public class DataService
{

    private SQLiteConnection _connection;

    public DataService(string DatabaseName)
    {

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", DatabaseName);
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, DatabaseName);

        if (!File.Exists(filepath))
        {
            Debug.Log("Database not in Persistent path");
            // if it doesn't ->
            // open StreamingAssets directory and load the db ->

#if UNITY_ANDROID
            var loadDb = new WWW("jar:file://" + Application.dataPath + "!/assets/" + DatabaseName);  // this is the path to your StreamingAssets in android
            while (!loadDb.isDone) { }  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check
            // then save to Application.persistentDataPath
            File.WriteAllBytes(filepath, loadDb.bytes);
#elif UNITY_IOS
                 var loadDb = Application.dataPath + "/Raw/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);
#elif UNITY_WP8
                var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
                // then save to Application.persistentDataPath
                File.Copy(loadDb, filepath);

#elif UNITY_WINRT
		var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
		
#elif UNITY_STANDALONE_OSX
		var loadDb = Application.dataPath + "/Resources/Data/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
		// then save to Application.persistentDataPath
		File.Copy(loadDb, filepath);
#else
	var loadDb = Application.dataPath + "/StreamingAssets/" + DatabaseName;  // this is the path to your StreamingAssets in iOS
	// then save to Application.persistentDataPath
	File.Copy(loadDb, filepath);

#endif

            Debug.Log("Database written");
        }

        var dbPath = filepath;
#endif
        _connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);
        Debug.Log("Final PATH: " + dbPath);

    }

    public void CreateGameplay()
    {

        //TODO: Linking it do the expermental parameters we have 
        Gameplay gamePlay = new Gameplay
        {
            street_direction = ExperementParameters.streetsDirections,
            pathes_per_direction = ExperementParameters.numberOfPathsPerStreet,
            car_speed_km = ExperementParameters.carsSpeed,
            car_span_km = ExperementParameters.distanceBetweenCars,
            sound_mode = ExperementParameters.soundDirections,
            player_name = "test",
            player_height = (double)ExperementParameters.lengthOfPatient
        };
        _connection.Insert(gamePlay);
    }
	public void CreateRoadCrossingData(int gameplay_id,
                                                string traffic_flow_towards,int current_time_span,
                                                                 double current_distance_nearest_car, bool gazing_car,
                                                                                           bool gazing_nearest_car, bool after_collision_frame)
	{
		   StreetCrossingData gameplayObject = new StreetCrossingData
        {
           gameplay_id= gameplay_id,
         traffic_flow_towards=   traffic_flow_towards,
        current_time_span =    current_time_span,
        current_distance_nearest_car =  current_distance_nearest_car,
          gaze_car= gazing_car,
           gaze_nearest_car= gazing_nearest_car,
         after_collision_frame =   after_collision_frame
        };
        _connection.Insert(gameplayObject);
	}



    /*******************************************return table in array form so we can extract it using foreach************************************/
    public IEnumerable<Gameplay> GetGameplayTable()
    {
        return _connection.Table<Gameplay>();
    }
    public IEnumerable<StreetCrossingData> GetRoadCrossingDataTable()
    {
        return _connection.Table<StreetCrossingData>();
    }
    public IEnumerable<GrabbedObjects> GetGrabbedObjectDataTable()
    {
        return _connection.Table<GrabbedObjects>();
    }

	/***********************************************GET DATA FROM DB******************************************** */
    public Gameplay GetGameplay() //getting the first row for gameplay data 
    {
        return _connection.Table<Gameplay>().Where(x => x.gameplay_id == 1).FirstOrDefault();
    }
}
