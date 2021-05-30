[<AutoOpen>]
module Farmer.Builders.ActionGroup

open Farmer
open Farmer.Arm

type ActionGroupConfig =
    { Name: ResourceName
      ShortName: string
      Tags: Map<string,string>
      EmailReceivers: EmailReceiver list }
   
    interface IBuilder with
        member this.ResourceId = accounts.resourceId this.Name
        member this.BuildResources location = [
            { Name = this.Name
              Location = location
              ShortName = this.ShortName
              Tags = this.Tags
              EmailReceivers = this.EmailReceivers }
       ]

type ActionGroupBuilder () =
    member _.Yield _ =
        { Name = ResourceName.Empty
          ShortName = ""
          Tags = Map.empty
          EmailReceivers = [] }
    [<CustomOperation "name">]
    member _.Name (state: ActionGroupConfig, name) = { state with Name = ResourceName name }
    [<CustomOperation "shortname">]
    member _.ShortName (state: ActionGroupConfig, shortname) = { state with ShortName = shortname }
    [<CustomOperation "emailreceivers">]
    member _.EmailReceivers (state: ActionGroupConfig, emailReceiver) = { state with EmailReceivers = emailReceiver }
    interface ITaggable<ActionGroupConfig> with member _.Add state tags = { state with Tags = state.Tags |> Map.merge tags }

let actionGroup = ActionGroupBuilder()