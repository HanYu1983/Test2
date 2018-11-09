require! {
    request
    crypto
    moment
    "./brickpicker/tool": Tool
}

(err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobipro.com", "/v1/account/accounts", {}, "GET")
console.log res

info = JSON.parse(res)
console.log info
if info.status == "ok"
    account_id = info.data[0].id
    
    #(err, res) <- Tool.getUrl huobiSignedOption("api.huobipro.com", "/v1/account/accounts/#{account_id}/balance", {}, "GET")
    #console.log res
    
    #
    (err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobipro.com", "/v1/order/orders/place", {"account-id": account_id, type: "buy-limit", amount: 1, symbol:"btcusdt", price: 1}, "POST")
    console.log res

(err, res) <- Tool.getUrl Tool.huobiSignedOption("api.huobipro.com", "/v1/order/orders", {symbol:"btcusdt", states: "submitted,partial-filled"}, "GET")
console.log res


