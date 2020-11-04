// ---------- Domain
type Label = string
type VitalForce = {power:int}
type Side =
  | Left
  | Right
type DeadOrAlive =
  | Dead
  | Alive of VitalForce
type Arm = Arm of Side
type Leg = Leg of Side
type BodyPart =
  | ArmPart of Arm * DeadOrAlive
  | LegPart of Leg * DeadOrAlive
  | TorsoPart of DeadOrAlive
  | HeadPart of DeadOrAlive
type Alive = BodyPart * VitalForce
type Dead = BodyPart
type Pay = VitalForce * BodyPart -> VitalForce * VitalForce option
// Functions
let pay (bodypart:BodyPart) (payment:VitalForce) =
  let cost = 
    match bodypart with
    | ArmPart (_,Dead) -> {power = 1}
    | LegPart (_,Dead) -> {power = 1}
    | TorsoPart Dead -> {power = 3}
    | HeadPart Dead -> {power = 5}
    | _ -> {power = 0}
  match cost, payment with
  | c,p when payment.power > cost.power -> Some c, p
  | _ -> None, payment
    
let createLife (deadBodyPart:BodyPart)(inputVitalForce:VitalForce)  =
  let paymentOption, restVitalForce = pay deadBodyPart inputVitalForce
  let tryCreate =
    match paymentOption with
    | Some payment ->
      match deadBodyPart with
      | ArmPart (side, Dead) -> ArmPart (side, Alive payment)
      | LegPart (side, Dead) -> LegPart (side, Alive payment)
      | TorsoPart Dead -> TorsoPart (Alive payment)
      | HeadPart Dead -> HeadPart (Alive payment)
      | x -> x
    | None -> deadBodyPart
  tryCreate, restVitalForce
// Flow
let vitalForceInit = {power = 5}
let bodyParts = [
  ArmPart (Arm Left, Dead)
  ArmPart (Arm Right, Dead)
  LegPart (Leg Right, Dead)
  LegPart (Leg Left, Dead)
  TorsoPart (Dead)
  HeadPart (Dead)
]

