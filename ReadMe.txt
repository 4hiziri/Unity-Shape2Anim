Shape2AnimフォルダをAssets直下に入れると使えるようになる。

注意: Assets/にファイルを書き出すとき同名ファイルは上書きされるので事前に移動させること

# なにこれ

Skinned Mesh RendererのBlendShapeの値が0フレーム目に設定されたAnimationをAssets/に生成するUnityエディタ拡張。
生成されるときはそのオブジェクトの名前.animで生成される。
BodyのBlendShapesから生成したらBody.animが生成される。
統合ツールが嫌いで表情をコンパクトに作りたい、そんなあなたに。

# 使い方

AnimationにしたいSkinned Mesh RendererをInspector上で右クリックするとメニューに出てくる。
以下にそれぞれの挙動を説明する。

1. Write all blend shape to Animation clip

全てのBlendShapeの値を書き込む、値が0のものも書き込むしvrc.v_aaとかも書き込む。

2. Write non-zero blend shape to Animation clip

値が0でないBlendShapeのみを書き込む。

3. Write blend shape except vrc.* to Animation clip

vrc.から始まるBlendShape以外を書き込む。

# ライセンス

MIT

# リポジトリ

https://github.com/4hiziri/Unity-Shape2Anim