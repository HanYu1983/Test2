# 使用方式

## 建立react-native 專案
1. npm i react-native -g
1. react-native init reactDev
1. cd reactDev
1. npm i create-react-class

## 編譯cljs到react-native
1. npm i shadow-cljs -g
1. cd cljsDev
1. npm i
1. npm start
1. reactDev中產生了app資料夾和編譯過的js檔在其中

## 套用cljs
將reactDev/index.js的內容改為以下

    /*
    import {AppRegistry} from 'react-native';
    import App from './App';
    import {name as appName} from './app.json';

    AppRegistry.registerComponent(appName, () => App);
    */
    import "./app/index.js";

執行react-native run-android
完成

# 注意
cljsDev/src/core.cljs中

    (defn start
        {:dev/after-load true}
        []
        (render-root "reactDev" (r/as-element [root])))

"reactDev"必須和reactDev/package.json中的name一樣, 不然會有AppRegistry沒有呼叫的錯誤