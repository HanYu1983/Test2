<- $

setTimeout do
    ->
        console.log "cmoney"
        
        doms = $('#InventoryDetailListInfo > tr')
        

        doms = $('#InventoryDetailListData > tr')
        console.log doms
        for dom in doms
            [have, cost, price, _, _, btn]:datas = $(dom).find("td")
            $(btn).find("input[value='平倉']").click()
            console.log datas
            break
    
    5000