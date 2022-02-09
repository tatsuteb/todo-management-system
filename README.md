## はじめに
このプロジェクトはZennに投稿した記事[「とにかくドメイン駆動設計を実践してみる試み ～TODO管理システム編～」](https://zenn.dev/tatsuteb/articles/f2d05abb8ce9a6)で取り上げたものです。

## デモ
https://user-images.githubusercontent.com/23710529/152676017-9fbd98ea-3235-4e0d-b858-d41036ab59b4.mp4

### Docker で動かす場合
1. GitHubからプロジェクトをクローン
2. クローンしたプロジェクト内のsrcフォルダに移動
3. 以下のコマンドを実行
```bash
> docker build -t tatsuteb/todo:demo .
> docker run -d --name tatsuteb-todo -p 80:80 -d tatsuteb/todo:demo
```
4. ブラウザで http://localhost へアクセス
5. 停止する場合は以下のコマンドを実行
```bash
> docker stop tatsuteb-todo
```
6. コンテナを削除してクリーンアップする場合は以下のコマンドを実行
```bash
> docker rm tatsuteb-todo
```

### .NET Core SDK で動かす場合
1. GitHubからプロジェクトをクローン
2. クローンしたプロジェクト内のsrcフォルダに移動
3. 以下のコマンドを実行
```bash
> dotnet run --project ./WebClient
```
4. ブラウザで http://localhost:5000 へアクセス
5. 停止する場合は Ctrl+C
