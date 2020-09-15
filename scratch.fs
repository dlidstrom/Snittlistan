open System

//type Rating = Rating of v : double
//type RatingDeviation = RatingDeviation of v : double
type My = My of v : double
type Phi = Phi of v : double
type Rating = {
    R : float
    RD : float
    Score : float
}
type GlickoRating = {
    My : My
    Phi : Phi
    Score : float
}

let toGlicko r = {
    My = My ((r.R - 1500.) / 173.7178)
    Phi = Phi (r.RD / 173.7178)
    Score = r.Score
}
let computeG glickoRating =
    let (Phi s) = glickoRating.Phi
    1. / Math.Sqrt(1. + 3. * s * s / (Math.PI * Math.PI))
let computeE my glickoRating =
    let (My myV) = my
    let (My myJV) = glickoRating.My
    1. / (1. + Math.Exp(-(computeG glickoRating) * (myV - myJV)))
let computeVariance my (opponentRatings : GlickoRating list) =
    let calcItem glickoRating =
        let g = computeG glickoRating
        let e = computeE my glickoRating
        let result = g * g * e * (1. - e)
        printfn "%.4f^2(%.4f)(1 - %.4f) = %.4f" g e e result
        result
    let result = 1. / (opponentRatings |> List.sumBy calcItem)
    result
let computeDelta v my (opponentRatings : GlickoRating list) =
    v * (opponentRatings |> List.sumBy (fun r -> (computeG r) * (r.Score - (computeE my r))))
let my = My 0.
let ratings = [
    { R = 1400.; RD = 30.; Score = 1. }
    { R = 1550.; RD = 100.; Score = 0. }
    { R = 1700.; RD = 300.; Score = 0. }
]
let glickoRatings = ratings |> List.map toGlicko
let gs = glickoRatings |> List.map (computeG)
let es = glickoRatings |> List.map (computeE my)
let myJ = My -0.5756
let phiJ = Phi (30. / 173.7178)
let v = computeVariance my glickoRatings
let x = {|
    v = v
    delta = computeDelta v my glickoRatings
|}
printfn "%A" x
