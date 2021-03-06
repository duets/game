module Simulation.Tests.Time.AdvanceTime_Tests

open System
open FsUnit
open Fugit.Shorthand
open NUnit.Framework

open Entities
open Simulation.Queries
open Simulation.Time.AdvanceTime

let dateMatches (expected: DateTime) (TimeAdvanced actual) =
    expected.Year |> should equal actual.Year
    expected.Month |> should equal actual.Month
    expected.Day |> should equal actual.Day
    TimeAdvanced actual

let hourMatches (hour: int) (TimeAdvanced date) = date.Hour |> should equal hour

[<Test>]
let ``advanceDayMoment should return next day moment`` () =
    let effects =
        advanceDayMoment Calendar.gameBeginning 1

    effects |> should haveLength 1

    List.head effects
    |> dateMatches Calendar.gameBeginning
    |> hourMatches 10

[<Test>]
let ``advanceDayMoment should roll over next day if current day moment is midnight``
    ()
    =
    let initialDate =
        Calendar.gameBeginning
        |> Calendar.withDayMoment Night

    let effects = advanceDayMoment initialDate 1

    effects |> should haveLength 1

    List.head effects
    |> dateMatches (Calendar.gameBeginning + oneDay)
    |> hourMatches 0

[<Test>]
let ``advanceDayMoment 2 should return 2 day moments later`` () =
    let effects =
        advanceDayMoment Calendar.gameBeginning 2

    effects |> should haveLength 2

    effects
    |> List.rev
    |> List.head
    |> dateMatches Calendar.gameBeginning
    |> hourMatches 14
