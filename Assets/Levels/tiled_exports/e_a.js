(function(name,data){
 if(typeof onTileMapLoaded === 'undefined') {
  if(typeof TileMaps === 'undefined') TileMaps = {};
  TileMaps[name] = data;
 } else {
  onTileMapLoaded(name,data);
 }})("e_a",
{ "height":10,
 "layers":[
        {
         "data":[1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 10, 10, 10, 10, 10, 10, 10, 10, 10, 1, 1, 2, 3, 4, 3, 4, 5, 6, 7, 8, 7, 6, 7, 8, 9, 10, 2, 3, 7, 1, 1, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19, 4, 12, 0, 0, 1, 1, 31, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 41, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 31, 0, 0, 33, 34, 35, 36, 37, 36, 37, 38, 0, 0, 0, 0, 0, 0, 0, 1, 1, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 59, 65, 52, 0, 0, 1, 1, 31, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 70, 51, 52, 0, 1, 1, 51, 63, 64, 65, 66, 67, 68, 63, 64, 65, 66, 67, 68, 69, 70, 61, 62, 63, 1, 1, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70],
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
                 "height":320,
                 "id":6,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":96,
                 "x":0,
                 "y":0
                }, 
                {
                 "height":64,
                 "id":9,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":544,
                 "x":96,
                 "y":0
                }, 
                {
                 "height":64,
                 "id":10,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":544,
                 "x":96,
                 "y":256
                }, 
                {
                 "height":64,
                 "id":14,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":96,
                 "x":480,
                 "y":192
                }, 
                {
                 "height":32,
                 "id":16,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":32,
                 "x":576,
                 "y":224
                }, 
                {
                 "height":32,
                 "id":17,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":256,
                 "x":160,
                 "y":160
                }, 
                {
                 "height":32,
                 "id":19,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":96,
                 "x":480,
                 "y":64
                }],
         "opacity":1,
         "type":"objectgroup",
         "visible":true,
         "width":20,
         "x":0,
         "y":0
        }],
 "nextobjectid":20,
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