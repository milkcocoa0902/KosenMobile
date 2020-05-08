using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Android.Content;

using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Util;

using Newtonsoft.Json;

namespace KosenMobile.src.rss{
  class DataManager{
    HttpClient client_;
    Context context_;
    public DataModel dataModel_;

    public DataManager(Context _context){
      dataModel_ = new DataModel(_context);
      context_ = _context;
      if (!File.Exists(dataModel_.databasePath_)) {
        CreateNew().Wait();
        Log.Debug("RSS.DataManager", "Create New Database");
      } else {
        Update();
      }

      Load().Wait();
        Log.Debug("RSS.DataManager", "Load Database");
    }

   async Task CreateNew()
    {
     await Task.Run(async () =>
      {
        client_ = new HttpClient();
        using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri("https://me.milkcocoa.info/rss.db")))
        using (var response = await client_.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
        {

          if (response.StatusCode == HttpStatusCode.OK)
          {

            var content = response.Content;
            var stream = await content.ReadAsStreamAsync();
            var path = dataModel_.databasePath_;
            var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.None);


            stream.CopyTo(fileStream);
          }
        }

    }

    public async Task Update()
    {

      client_ = new HttpClient();
      var endpoint = "http://kosenmobile.milkcocoa.info/update/";
      var count = dataModel_.dataRef_.Count;
      var response = await client_.GetAsync(endpoint + count.ToString());
      var jsonString = await response.Content.ReadAsStringAsync();
      var json = JsonConvert.DeserializeObject<List<DataModel.Model>>(jsonString);
      dataModel_.adddata(json);
    }

    bool CheckForUpdate()
    {
      return false;
    }

    async Task Load()
    {
      Task.Run(() => {
        dataModel_.read();
      }).Wait();
    }
  }
}