binance = require("node-binance-api")().options do
    APIKEY: "vmPUZE6mv9SD5VNHk4HlWFsOr6aKE2zvsw0MuIgwCIPy6utIco14y7Ju91duEh8A"
    APISECRET: "NhqPtmdSJYdKjVHjA7PZj4Mge3R5YNiP1e3UZjInClVN65XAbvqqM6A7H5fATj0j"
    useServerTime: true
    test: true

binance.prices do
    (error, ticker)->
        console.log("Price of BNB: ", ticker);

/*
binance.balance do
    (error, balances)->
        # 沒有正確的apikey
        if error
            console.error("body", error.body)
            return
        console.log "balances()" balances
        console.log "ETH balance" balances.ETH.available
*/



binance.buy("BNBETH", 1, 0, {type:'LIMIT'}, (error, response) ->
    if(error)
        console.log(error.body)
    console.log("Limit Buy response", response);
    console.log("order id: " + response.orderId);
)

binance.cancel("ETHBTC", 0, (error, response, symbol) ->
    if(error)
        console.log(error.body)
    console.log(symbol+" cancel response:", response);
)

binance.openOrders("ETHBTC", (error, openOrders, symbol) ->
    if(error)
        console.log(error.body)
    console.log("openOrders("+symbol+")", openOrders);
)

binance.orderStatus("ETHBTC", 0, (error, orderStatus, symbol) ->
    if(error)
        console.log(error.body)
    console.log(symbol+" order status:", orderStatus);
);