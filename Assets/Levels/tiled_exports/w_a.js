(function(name,data){
 if(typeof onTileMapLoaded === 'undefined') {
  if(typeof TileMaps === 'undefined') TileMaps = {};
  TileMaps[name] = data;
 } else {
  onTileMapLoaded(name,data);
 }})("w_a",
{ "height":10,
 "layers":[
        {
         "data":[70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 3, 4, 5, 6, 3, 4, 5, 6, 7, 8, 9, 10, 70, 70, 70, 70, 70, 70, 70, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19, 9, 10, 70, 70, 70, 70, 70, 70, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19, 9, 10, 70, 70, 70, 70, 70, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 19, 3, 4, 5, 6, 7, 20, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 30, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 40, 70, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 50, 70, 63, 64, 65, 66, 67, 68, 63, 64, 65, 66, 67, 68, 63, 64, 65, 66, 67, 68, 60, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70],
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
         "height":0,
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
                 "width":320,
                 "x":320,
                 "y":64
                }, 
                {
                 "height":32,
                 "id":3,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":288,
                 "x":352,
                 "y":96
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
                 "width":256,
                 "x":384,
                 "y":128
                }, 
                {
                 "height":64,
                 "id":5,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":640,
                 "x":0,
                 "y":256
                }, 
                {
                 "height":96,
                 "id":6,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":64,
                 "x":576,
                 "y":160
                }],
         "opacity":1,
         "type":"objectgroup",
         "visible":true,
         "width":0,
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