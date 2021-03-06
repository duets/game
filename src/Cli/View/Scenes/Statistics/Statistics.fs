module Cli.View.Scenes.Statistics.Root

open Cli.View.Actions
open Cli.View.Common
open Cli.View.TextConstants

let statisticOptions =
    [ { Id = "band"
        Text = TextConstant StatisticsSectionBand }
      { Id = "albums"
        Text = TextConstant StatisticsSectionAlbums } ]

let rec statisticsScene () =
    seq {
        yield Figlet <| TextConstant StatisticsTitle

        yield
            Prompt
                { Title = TextConstant StatisticsSectionPrompt
                  Content =
                      ChoicePrompt
                      <| OptionalChoiceHandler
                          { Choices = statisticOptions
                            Handler =
                                rehearsalRoomOptionalChoiceHandler
                                <| processSelection
                            BackText = TextConstant CommonCancel } }

    }

and processSelection selection =
    seq {
        match selection.Id with
        | "band" -> yield SubScene StatisticsOfBand
        | "albums" -> yield SubScene StatisticsOfAlbums
        | _ -> yield NoOp
    }
