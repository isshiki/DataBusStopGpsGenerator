using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication11 {

  class BusStop {
    public string busstopname;  // バス停名
    public string buslinename;  // バス路線名
  }

  class Program {
    static void Main(string[] args) {

      // TODO: それぞれ適切なファイルパスに記述し直してください。
      var gps1          = @"C:\Projects\ArticleSamples\Wikitude02\OriginData\gps1.txt";
      var gps2          = @"C:\Projects\ArticleSamples\Wikitude02\OriginData\gps2.txt";
      var csvAll        = @"C:\Projects\ArticleSamples\Wikitude02\MyJsonData\dataBusStopGps.csv";
      var jsonAll       = @"C:\Projects\ArticleSamples\Wikitude02\MyJsonData\myjsondata.all.js";
      var jsonTokyo     = @"C:\Projects\ArticleSamples\Wikitude02\MyJsonData\myjsondata.tokyo.js";
      var jsonIkebukuro = @"C:\Projects\ArticleSamples\Wikitude02\MyJsonData\myjsondata.ikebukuro.js";

      var gps1Array = File.ReadAllLines(gps1);
      var gps2Array = File.ReadAllLines(gps2);

      // バス停情報を集計
      var listBusStop = new Dictionary<string, BusStop>();
      foreach (var businfoLine in gps2Array) {
        var businfoEach = businfoLine.Split(',');
        if (businfoEach.Length != 3) {
          Console.WriteLine("BUS不正データ： " + businfoLine);
        }
        var id = businfoEach[0];
        if (listBusStop.ContainsKey(id)) {
          var existingData = listBusStop[id];
          existingData.buslinename = existingData.buslinename + "|" + businfoEach[2];
          listBusStop[id] = existingData;
        } else {
          listBusStop.Add(id, new BusStop() { busstopname = businfoEach[1], buslinename = businfoEach[2] } );
        }
      }

      // バス停のGPSデータを完成させる
      var dataBusStopGpsCsv = new StringBuilder();
      var dataBusStopGpsAll = new StringBuilder();
      var dataBusStopGpsTokyo = new StringBuilder();
      var dataBusStopGpsIkebukuro = new StringBuilder();

      var jsonPreText = "var myJsonData = [";
      var jsonPostText = "\r\n];\r\n";

      dataBusStopGpsAll.Append(jsonPreText);
      dataBusStopGpsTokyo.Append(jsonPreText);
      dataBusStopGpsIkebukuro.Append(jsonPreText);

      foreach (var gpsinfoLine in gps1Array) {
        var gpsinfoEach = gpsinfoLine.Split(',');
        if (gpsinfoEach.Length != 3) {
          Console.WriteLine("GPS不正データ： " + gpsinfoLine);
        }

        string jsonPreLineEnd = ",\r\n";

        var id = gpsinfoEach[0];
        var latitude = gpsinfoEach[1]; //（緯度）
        var longitude = gpsinfoEach[2]; //（経度）
        var busstopname = "";
        var buslinename = "";
        if (listBusStop.ContainsKey(id)) {
          var busStopInfo = listBusStop[id];
          busstopname = busStopInfo.busstopname;
          buslinename = busStopInfo.buslinename;
        } else {
          // エラー!!!
          Console.WriteLine("LST不正データ： " + id);
        }

        // JSONデータの見だし： id, latitude（緯度）, longitude（経度）, busstopname（バス停名）, buslinename（バス路線名） ※なお、altitude（高度）の情報はありません。
        dataBusStopGpsCsv.AppendFormat("{0},{1},{2},{3},{4}", id, latitude, longitude, busstopname, buslinename);
        dataBusStopGpsAll.AppendFormat(jsonPreLineEnd + "	{{ \"id\": \"{0}\", \"latitude\": {1}, \"longitude\": {2}, \"busstopname\": \"{3}\", \"buslinename\": \"{4}\" }}", id, latitude, longitude, busstopname, buslinename);

        // フィルタリング。日本における1kmあたりの緯度・経度はそれぞれ約0.01度程度になるので、便宜上、ある地点（東京駅、池袋駅）の周囲3kmとして、その前後の0.03度の範囲のみに絞ってデータを軽くしています。
        decimal latitudeValue = 0.0m; Decimal.TryParse(latitude, out latitudeValue);
        decimal longitudeValue = 0.0m; Decimal.TryParse(longitude, out longitudeValue);

        // 参考1：東京駅の緯度「35.681368」、経度「139.766076」
        if (((35.681368m - 0.03m) < latitudeValue) && (latitudeValue < (35.681368m + 0.03m)) &&
            ((139.766076m - 0.03m) < longitudeValue) && (longitudeValue < (139.766076m + 0.03m))) {
          dataBusStopGpsTokyo.AppendFormat(jsonPreLineEnd + "	{{ \"id\": \"{0}\", \"latitude\": {1}, \"longitude\": {2}, \"busstopname\": \"{3}\", \"buslinename\": \"{4}\" }}", id, latitude, longitude, busstopname, buslinename);
        }

        // 参考2：池袋駅の緯度「35.72888」、経度「139.7081588」
        if (((35.72888m - 0.03m) < latitudeValue) && (latitudeValue < (35.72888m + 0.03m)) &&
            ((139.7081588m - 0.03m) < longitudeValue) && (longitudeValue < (139.7081588m + 0.03m))) {
          dataBusStopGpsIkebukuro.AppendFormat(jsonPreLineEnd + "	{{ \"id\": \"{0}\", \"latitude\": {1}, \"longitude\": {2}, \"busstopname\": \"{3}\", \"buslinename\": \"{4}\" }}", id, latitude, longitude, busstopname, buslinename);
        }
      }

      dataBusStopGpsAll.Append(jsonPostText);
      dataBusStopGpsTokyo.Append(jsonPostText);
      dataBusStopGpsIkebukuro.Append(jsonPostText);

      File.WriteAllText(csvAll, dataBusStopGpsCsv.ToString().Replace("[,", "["));
      File.WriteAllText(jsonAll, dataBusStopGpsAll.ToString().Replace("[,", "["));
      File.WriteAllText(jsonTokyo, dataBusStopGpsTokyo.ToString().Replace("[,", "["));
      File.WriteAllText(jsonIkebukuro, dataBusStopGpsIkebukuro.ToString().Replace("[,", "["));

    }
  }
}
