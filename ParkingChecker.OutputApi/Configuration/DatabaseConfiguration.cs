using System;

namespace ParkingChecker.OutputApi.Configuration
{
    public interface IDatabaseConfiguration
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
    }
    public class DatabaseConfiguration : IDatabaseConfiguration
    {   
        public DatabaseConfiguration(){
            
            _connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING") ?? null;;
            _databaseName = Environment.GetEnvironmentVariable("DATABASE") ?? null;  
        }
        private string _databaseName {get; set;}
        private string _connectionString{get; set;}
        public string DatabaseName 
        { 
            get { return _databaseName;}
            set 
            { 
                if(_databaseName == null)
                {
                    _databaseName = value;
                }
            } 
        }
        public string ConnectionString 
        { 
            get {return _connectionString; } 
            set 
            {
                if(_connectionString == null)
                {
                    _connectionString = value;
                }
            } 
        }
    }
}