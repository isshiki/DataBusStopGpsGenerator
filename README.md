# DataBusStopGpsGenerator

バス停のGPSデータをJSONデータ（厳密にはJavaScriptオブジェクトリテラル）形式にして出力するツール。

連載記事「[Wikitudeで「オープンデータ＋AR（拡張現実）」スマホアプリをお手軽開発［PR］ - Build Insider](http://www.buildinsider.net/pr/grapecity/wikitude)」でデータ作成時に使用したC#のコンソールアプリ。

## 入力データの作り方

バス亭GPSの元データは、[バス停留所（国土交通省）](http://nlftp.mlit.go.jp/ksj/gml/datalist/KsjTmplt-P11.html)を使います。  
リンク先のページで［東京］を指定して取得したXMLファイルをExcelで開き、「id, latitude（緯度）, longitude（経度）」と「id, busstopname（バス停名）, buslinename（バス路線名）」という列を使って、新たに2つのcsvファイルを作成します。  
その2つのcsvファイルのデータを、このコンソロールアプリで再び1つのデータセットに統合します。

2つのcsvに分ける理由は、1つのバス停に対して複数のバス路線が存在するためです。

## ライセンス

MIT.



