# DataBusStopGpsGenerator

バス停のGPSデータをJSONデータ（厳密にはJavaScriptオブジェクトリテラル）形式にして出力するツール。

連載記事「[Wikitudeで「オープンデータ＋AR（拡張現実）」スマホアプリをお手軽開発［PR］ - Build Insider](http://www.buildinsider.net/pr/grapecity/wikitude)」でデータ作成時に使用したC#のコンソールアプリ。

## 入力データの作り方

バス亭GPSの元データは、[バス停留所（国土交通省）](http://nlftp.mlit.go.jp/ksj/gml/datalist/KsjTmplt-P11.html)を使いました。
リンク先のページで［東京］を指定して取得したXMLファイルをExcelで開き、2つのcsvファイルにまとめました。
その2分されたデータで、このコンソロールアプリで1つのデータセットに統合しています。


## ライセンス

MIT.



