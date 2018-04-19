LiveAvatar
========================

HMDを使わずにバーチャルユーチューバーになれるキットです

## ハードウェア要件

+ HTC Vive(ベースステーション + コントローラーのみ)
+ Webカメラ(視野角60°ぐらいを想定)
+ モニタ(2画面)

## HMD無しでSteamVRを起動する

以下を参考に手順を実施すると使えます。

### SteamVRのベータ版をインストール

[【Unity】VR ReadyでないPCでVive Trackerを使う | てんちょーの技術日誌](http://shop-0761.hatenablog.com/entry/2018/03/30/231703)

### HMD必須にしない設定変更
[【Unity】ViveのHMDなしにTrackerを使う | てんちょーの技術日誌](http://shop-0761.hatenablog.com/entry/2018/01/08/034418)

### Init_HmdNotFoundPresenceFailed エラーを潰す

[HTC Vive TrackerをUnityでHMDレスで使いたい時に出るエラーを回避する | しゃの](http://d.hatena.ne.jp/syan0/20180316/1521199910)

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

## 操作方法

### Webカメラからの操作

見てる項目

+ 顔の向き、距離
+ 口の開き具合

### キーボード操作

|キー|効果|
|---|---|
|I|キャリブレーションモード ON/OFF|
|H|エモーションのヘルプ表示|
|↑↓|台詞の選択|
|Space|台詞の表示/非表示|
|G|グリーンバックの切り替え|
|F|手のポーズの操作変更(test版:4方向+トリガーシフト/8方向)|

### Viveコントローラ操作

|操作|効果|
|---|---|
|右手メニューボタン<br>+<br>左手メニューボタン|キャリブレーションモードON|
|キャリブレーションモード中に<br>両手を広げて右手トリガー|キャリブレーション|
|右手メニューボタン|カメラ目線ON/OFF|
|右手トラックパッドクリック<br>の後にトラックパッドをスライド|表情切替え(表情は下図参照)|
|左手メニューボタン|台詞ウィンドウのON/OFF|
|左手トラックパッドの上下クリック|台詞ウィンドウの選択移動|
|左手トリガー|台詞表示|
|グリップボタンを押しながら<br>トラックパッドをスライド|手のポーズ切替(ポーズは下図参照)|
|グリップボタンを押しながら<br>さらにトリガーを引きつつ<br>トラックパッドをスライド|手のポーズ切替(ポーズは下図参照)|

### エモーション図解
![エモーション](https://github.com/m2wasabi/LiveAvatar/raw/ViveController/Assets/FaceTrack/Images/manual.png)

### 台詞のインストール

`LiveAvatar_Data/scenario.txt`(Editor上ではAssets直下) ファイルを作成し、
テキストを書く(文字コードはUTF-8で)

## ビルド方法

### Asset StoreからのAssetインポート

以下のアセットを導入する(有料の物が多いです)

#### UniRx
https://assetstore.unity.com/packages/tools/unirx-reactive-extensions-for-unity-17276

#### SteamVR Plugin
https://assetstore.unity.com/packages/templates/systems/steamvr-plugin-32647
#### Final IK
https://assetstore.unity.com/packages/tools/animation/final-ik-14290

#### Dlib FaceLandmark Detector
https://assetstore.unity.com/packages/tools/integration/dlib-facelandmark-detector-64314

#### Dynamic Bone
https://assetstore.unity.com/packages/tools/animation/dynamic-bone-16743

#### Ai
https://assetstore.unity.com/packages/3d/characters/ai-80561

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
