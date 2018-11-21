require! {
    fs
    crypto
}

export class FSProvider
    ({cacheDir})->
        @cacheDir = cacheDir
    
    get: (key, cb)->
        urlKey = crypto.createHash('md5').update(key).digest('hex')
        path = "#{@cacheDir}#{urlKey}.html"
        console.log "get:", path
        if not (fs.existsSync path)
            cb()
        else
            fs.readFile path, 'utf8', (err, txt)->
                try
                    # 注意！！把Buffer從int array轉換回來
                    json = JSON.parse txt
                    json.body = Buffer.from json.body.data
                    cb && cb(err, json)
                catch e
                    cb && cb()
    set: (key, cache, ttl, cb)->
        urlKey = crypto.createHash('md5').update(key).digest('hex')
        path = "#{@cacheDir}#{urlKey}.html"
        console.log "set:", path
        fs.writeFile path, JSON.stringify(cache), cb
    
    remove: (key, cb)->
        urlKey = crypto.createHash('md5').update(key).digest('hex')
        path = "#{@cacheDir}#{urlKey}.html"
        console.log "remove file #{path}"
        fs.unlink path, cb
    
    clear: (cb)->
        console.log "clear"
        cb && cb err
