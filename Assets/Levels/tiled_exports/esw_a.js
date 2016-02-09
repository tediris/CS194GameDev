(function(name,data){
 if(typeof onTileMapLoaded === 'undefined') {
  if(typeof TileMaps === 'undefined') TileMaps = {};
  TileMaps[name] = data;
 } else {
  onTileMapLoaded(name,data);
 }})("esw_a",
{ "height":10,
 "layers":[
        {
         "data":[70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 7, 8, 3, 4, 5, 6, 7, 8, 3, 4, 5, 6, 7, 8, 3, 4, 5, 6, 7, 8, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 33, 34, 35, 36, 37, 34, 35, 36, 37, 34, 35, 36, 37, 36, 37, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 65, 66, 67, 68, 52, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 59, 65, 66, 67, 68, 70, 70, 70, 70, 62, 52, 0, 0, 0, 0, 0, 0, 0, 0, 59, 69, 70, 70, 70, 70],
         "height":10,
         "name":"Tile Layer 1",
         "opacity":1,
         "type":"tilelayer",
         "visible":true,
         "width":20,
         "x":0,
         "y":0
        }, 
        {
         "draworder":"topdown",
         "height":10,
         "name":"Colliders",
         "objects":[
                {
                 "height":64,
                 "id":1,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":640,
                 "x":0,
                 "y":0
                }, 
                {
                 "height":32,
                 "id":2,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":512,
                 "x":64,
                 "y":160
                }, 
                {
                 "height":64,
                 "id":3,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":160,
                 "x":0,
                 "y":256
                }, 
                {
                 "height":32,
                 "id":4,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":32,
                 "x":160,
                 "y":288
                }, 
                {
                 "height":32,
                 "id":5,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":32,
                 "x":448,
                 "y":288
                }, 
                {
                 "height":64,
                 "id":6,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":160,
                 "x":480,
                 "y":256
                }],
         "opacity":1,
         "type":"objectgroup",
         "visible":true,
         "width":20,
         "x":0,
         "y":0
        }],
 "nextobjectid":7,
 "orientation":"orthogonal",
 "properties":
    {

    },
 "renderorder":"right-up",
 "tileheight":32,
 "tilesets":[
        {
         "columns":10,
         "firstgid":1,
         "image":"..\/..\/Tilemaps\/ground_sprites.png",
         "imageheight":224,
         "imagewidth":320,
         "margin":0,
         "name":"ground",
         "properties":
            {

            },
         "spacing":0,
         "tilecount":70,
         "tileheight":32,
         "tilewidth":32
        }],
 "tilewidth":32,
 "version":1,
 "width":20
});