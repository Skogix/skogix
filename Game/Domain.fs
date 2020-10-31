module Game.Domain
type Direction =
  | Up
  | Down
  | Left
  | Right
type Input =
  | Move of Direction
type Output =
  | TileMap 
