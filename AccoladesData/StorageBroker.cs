using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AccoladesData
{
    public class StorageBroker<T> : IStorageBroker<T> where T : class
    {
        private string file { get; set; }
        public string UserId { get; set; }

        public StorageBroker()
        {
            EntityName();
        }

        private void EntityName()
        {
            var fullJson = typeof(T).ToString().ToLower();
            var splitName = fullJson.Split(".").Reverse().ToArray();
            file = splitName[0];

        }

        private void CreateFile()
        {
            if (!File.Exists($"./Data/{file}_{UserId}.json"))
            {

                var filestream = File.Create($"./Data/{file}_{UserId}.json");
                filestream.Dispose();
            }
        }

        public void Delete(T entity)
        {
            var initialJson = File.ReadAllText($"./Data/{file}_{UserId}.json");

            List<T> nigerianStates = JsonConvert.DeserializeObject<List<T>>(initialJson);
            var comingEntity = JsonConvert.SerializeObject(entity);

            List<T> newList = new List<T>();
            foreach (var item in nigerianStates)
            {
                var itemToDel = JsonConvert.SerializeObject(item);

                if (itemToDel != comingEntity)
                {
                    newList.Add(item);
                }
            }

            var completeJson = JsonConvert.SerializeObject(newList);
            File.WriteAllText($"./Data/{file}_{UserId}.json", completeJson);
        }

        public IEnumerable<T> GetAll()
        {
            var initialJson = File.ReadAllText($"./Data/{file}_{UserId}.json"); //  C:\Projects\Waka\Data  ../Waka/Data/{file}.json  C:\Projects\WakaV2\WakaWeb
            List<T> nigerianStates = JsonConvert.DeserializeObject<List<T>>(initialJson);
            return nigerianStates;
        }

        public T Post(T entity)
        {
            CreateFile();

            var initialJson = File.ReadAllText($"./Data/{file}_{UserId}.json");
            if (initialJson.Length > 15)
            {
                List<T> publicPlaces = JsonConvert.DeserializeObject<List<T>>(initialJson);
                publicPlaces.Add(entity);
                var completeJson = JsonConvert.SerializeObject(publicPlaces);
                File.WriteAllText($"./Data/{file}_{UserId}.json", completeJson);
                
            }
            else
            {
                List<T> freshDb = new List<T>();
                freshDb.Add(entity);
                var completeJson = JsonConvert.SerializeObject(freshDb);
                File.WriteAllText($"./Data/{file}_{UserId}.json", completeJson);
            }

            return entity;
        }

        public void PostRange(List<T> entities)
        {
            CreateFile();

            foreach (var entity in entities)
            {
                var initialJson = File.ReadAllText($"./Data/{file}_{UserId}.json");
                if (initialJson.Length > 15)
                {
                    List<T> publicPlaces = JsonConvert.DeserializeObject<List<T>>(initialJson);
                    publicPlaces.Add(entity);
                    var completeJson = JsonConvert.SerializeObject(publicPlaces);
                    File.WriteAllText($"./Data/{file}_{UserId}.json", completeJson);
                }
                else
                {
                    List<T> freshDb = new List<T>();
                    freshDb.Add(entity);
                    var completeJson = JsonConvert.SerializeObject(freshDb);
                    File.WriteAllText($"./Data/{file}_{UserId}.json", completeJson);
                }
            }

        }

        public void Update(T entityToUpdate, T compareEntity)
        {
            var initialJson = File.ReadAllText($"./Data/{file}_{UserId}.json");

            if (string.IsNullOrWhiteSpace(initialJson))
            {
                Post(entityToUpdate);
            }
            else
            {
                List<T> entities = JsonConvert.DeserializeObject<List<T>>(initialJson);
                var dbState = JsonConvert.SerializeObject(compareEntity);
                List<T> newList = new List<T>();
                foreach (var item in entities)
                {
                    var itemToDel = JsonConvert.SerializeObject(item);

                    if (itemToDel != dbState)
                    {
                        newList.Add(item);
                    }
                }
                newList.Add(entityToUpdate);
                var completeJson = JsonConvert.SerializeObject(newList);
                File.WriteAllText($"./Data/{file}_{UserId}.json", completeJson);
            }

            
        }
    }
}
