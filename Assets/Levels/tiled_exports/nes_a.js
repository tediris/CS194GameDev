(function(name,data){
 if(typeof onTileMapLoaded === 'undefined') {
  if(typeof TileMaps === 'undefined') TileMaps = {};
  TileMaps[name] = data;
 } else {
  onTileMapLoaded(name,data);
 }})("nes_a",
{ "height":10,
 "layers":[
        {
         "data":[70, 70, 70, 70, 70, 21, 0, 0, 0, 0, 0, 0, 0, 0, 30, 70, 70, 70, 70, 70, 70, 70, 70, 70, 70, 31, 0, 0, 0, 0, 0, 33, 38, 0, 19, 3, 4, 5, 6, 7, 70, 70, 70, 70, 70, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 70, 70, 70, 70, 31, 0, 33, 34, 38, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 70, 70, 70, 70, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 70, 70, 70, 70, 31, 0, 0, 0, 0, 0, 0, 33, 34, 37, 38, 0, 0, 0, 0, 70, 70, 70, 70, 70, 21, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 70, 70, 70, 70, 31, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 70, 70, 70, 70, 70, 21, 0, 0, 33, 34, 37, 38, 0, 0, 59, 63, 64, 65, 66, 67, 70, 70, 70, 70, 70, 31, 0, 0, 0, 0, 0, 0, 0, 0, 50, 70, 70, 70, 70, 70],
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
                 "id":1,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":192,
                 "x":0,
                 "y":0
                }, 
                {
                 "height":64,
                 "id":2,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":192,
                 "x":448,
                 "y":0
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
                 "width":192,
                 "x":448,
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
                 "width":128,
                 "x":256,
                 "y":256
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
                 "width":128,
                 "x":384,
                 "y":160
                }, 
                {
                 "height":32,
                 "id":6,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":96,
                 "x":224,
                 "y":96
                }, 
                {
                 "height":32,
                 "id":7,
                 "name":"",
                 "properties":
                    {

                    },
                 "rotation":0,
                 "type":"",
                 "visible":true,
                 "width":64,
                 "x":352,
                 "y":32
                }],
         "opacity":1,
         "type":"objectgroup",
         "visible":true,
         "width":20,
         "x":0,
         "y":0
        }],
 "nextobjectid":8,
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