LiveAvatar
========================

HMDを使わずにバーチャルユーチューバーになれるキットです

## ハードウェア要件

+ HTC Vive(ベースステーション + コントローラーのみ)
+ Webカメラ(視野角60°ぐらいを想定)
+ モニタ(2画面)

※おすすめは、ゲーミングノートに外部モニタを1枚繋ぐ構成です。

## ソフトウェア要件

音声認識やボイロに喋らせたい場合、以下のソフトウェアが追加で必要になります。

+ Windows10 (Windowsの音声認識を使っています)
+ [SeikaCenter](https://hgotoh.jp/wiki/doku.php/documents/voiceroid/seikacenter/seikacenter-001)
+ [VOICEROID](https://www.ah-soft.com/voiceroid/)（+や2でも可）
+ CeVIOやSofTalkでもできるらしいけど未検証

## HMD無しでSteamVRを起動する

以下を参考に手順を実施すると使えます。

### SteamVRのベータ版をインストール

* Steamの『ライブラリ』から『SteamVR』を探して、『プロパティ』ウィンドウを開き、β版をインストールする。

> [【Unity】VR ReadyでないPCでVive Trackerを使う | てんちょーの技術日誌](http://shop-0761.hatenablog.com/entry/2018/03/30/231703)

### HMD必須にしない設定変更

以下のファイルを探し出し、テキストエディタで編集します。

`<Steamのインストール先>/steamapps/common/SteamVR/resources/settings/default.vrsettings`

編集するのは2項目。

+ `steamvr.requireHmd` : `false`
+ `steamvr.activateMultipleDrivers` : `true`

```
{
	"steamvr": {
		"requireHmd": false,
～中略～
		"activateMultipleDrivers": true,
～略～
```

> [【Unity】ViveのHMDなしにTrackerを使う | てんちょーの技術日誌](http://shop-0761.hatenablog.com/entry/2018/01/08/034418)
> [HTC Vive TrackerをUnityでHMDレスで使いたい時に出るエラーを回避する | しゃの](http://d.hatena.ne.jp/syan0/20180316/1521199910)

### 先にViveコントローラーの電源を入れてからStreamVRを起動する

1. Viveコントローラーの電源を入れる(LEDが緑に光る)
2. SteamVRを起動する

**失敗したら…**

1. SteamVRを終了する
2. Steamを終了する

**それでも安定しない場合(特にアップデート失敗)は…**

1. インターネットを切断した状態でSteamVRを起動する
2. オフラインモードで起動するか聞かれるので、オフラインモードにする
3. SteamVRが起動してコントローラーを認識した後、インターネットにつなぐ
4. PC再起動
5. アップデートに失敗する場合、一度のSteamVRモニタを直接起動する

`<Steamのインストール先>/steamapps/common/SteamVR/bin/win32/vrmonitor.exe` 


## 操作方法

### Webカメラからの操作

見てる項目

+ 顔の向き、距離
+ 口の開き具合

### キーボード操作

|キー|効果|
|---|---|
|ESC|設定メニュー呼び出し|
|TAB|VRMロード|
|Space|台詞の表示/非表示|

### Viveコントローラ操作

|操作|効果|
|---|---|
|トラックパッドクリック<br>の後にトラックパッドを軽くスライド|手のフォーム切替え1|
|トラックパッドクリック<br>の後にトラックパッドを重くスライド|手のフォーム切替え2|
|右手トリガーを引きながら<br>トラックパッドをスライド|表情切替え(表情Manual時)|
|左手トラックパッドの上下クリック|台詞ウィンドウの選択移動|
|左手トリガーを引く|台詞選択モード / 音声認識のON/OFF|
|左手トリガーを引きながら<br>左手トラックパッドの上下クリック|台詞選択|
|左手トリガーを放す|台詞決定|
|左手メニューボタン|台詞ウィンドウのON/OFF|

### エモーション図解
<s>![エモーション](https://github.com/m2wasabi/LiveAvatar/raw/ViveController/Assets/FaceTrack/Images/manual.png)</s>

### 台詞のインストール

`LiveAvatar_Data/scenario.txt`(Editor上ではAssets直下) ファイルを作成し、
テキストを書く(文字コードはUTF-8で)

### 音声認識

Windowsでマイクを認識させたあと、『Windowsの設定』→『プライバシー』→『音声認識、手書き入力、入力の設定』で【音声認識サービスをオンにします】をクリックする

### VOICEROID連携

1. VOICEROID*、SeikaCenter、SeikaServerCompact をインストールする
2. VOICEROID*を起動する
3. SeikaCenterを起動し、サービスを開始する
4. `SeikaServerCompact.json` を編集し、SeikaServerCompactを起動する(面倒なら `run.bat` から起動できます)

`SeikaServerCompact.json` の設定例

```
{
  "Addr": "*",
  "Port": 7180,
  "WorkDir": "./work",
  "HtmlDir": "./html",

  "User": "local",
  "Password": "password",

  "HtmlContents": {
    ".html": "text/html; charset=utf-8",
    ".css": "text/css; charset=utf-8",
    ".js": "text/javascript; charset=utf-8",
    ".json": "applocation/json; charset=utf-8",
    ".jpg": "image/jpeg",
    ".png": "image/png",
    ".gif": "image/gif",
    ".wav": "audio/wav",
    ".ico": "image/x-icon"
  }
}
```


## ビルド方法

### Asset StoreからのAssetインポート

以下のアセットを導入する(有料の物が多いです)

#### UniRx
https://assetstore.unity.com/packages/tools/unirx-reactive-extensions-for-unity-17276

#### SteamVR Plugin 1.2.3
https://github.com/ValveSoftware/steamvr_unity_plugin/releases/tag/1.2.3

AssetStoreからダウンロードすると2.0.x しか手に入らないので、GitHubから入手します

#### Final IK
https://assetstore.unity.com/packages/tools/animation/final-ik-14290

#### Dlib FaceLandmark Detector
https://assetstore.unity.com/packages/tools/integration/dlib-facelandmark-detector-64314

#### ”日本の教室”モデル
https://assetstore.unity.com/packages/3d/props/interior/japanese-classroom-model-71392

### Standard Assets からのインポート
#### Standard Assets
`Assets` -> `ImportPackage` -> `Characters` から
`Assets\Standard Assets\Characters\ThirdPersonCharacter\Animation.fbx/HumanoidIdle.fbx ` をインポートする

### 野良アセットの導入

#### HandPoses 0.2.0

拙作の `HandPoses_0.2.0.unitypackage` をインポートする
https://1drv.ms/f/s!AhjMMY3zrGQhuiNeoa6140bvKWyA

### フォントの導入

#### k-font

http://font.sumomo.ne.jp/font_1.html

`keifont.ttf` を `/Assets/fonts/` にコピーする

### Project Config

`Prefarences` → `SteamVR` → `Automatically Enable VR` を OFF

`Edit` → `Project Settings` → `Player` → `XR Settings` → `Virtual Reality Surported` を OFF

### ビルド

`ai_classroom_steamvr.unity` のシーンをビルドする

## うまくいかないときは(既知の問題)

### SteamVRが起動しているにもかかわらずコントローラーが反応しない

`openvr_api.dll` が生成されていない可能性があります。  
原因は分かっていませんが、 別途空のプロジェクトに `SteamVR Plugin` をインポート、デモシーンをビルドして、  
そこから `<アプリ名>_Data/Plugins/openvr_api.dll` をコピーすれば動きます。  

### Steamドングル経由で接続してしばらく放置すると、Vive コントローラーのトラッキングが外れて反応しなくなる

電池が足りないかSteamドングルが何らかの理由でコントローラーの信号を拾えなくなっています。

アプリケーションを終了し、SteamVRを終了(終わらないときはプロセスを落とす)し、Steamを終了。
コントローラーの電源を入れて、ランプが緑になったのを確認してからSteamVRを起動しましょう。  

## ライセンス

本リポジトリにて公開しているソースコードはMITライセンスに準拠します。  
