[<AutoOpen>]
module Farmer.Arm.ActionGroup

open Farmer

let resource =
    ResourceType("microsoft.insights/actionGroups", "2019-06-01")

type EmailReceiver = { Name: string; EmailAddress: string }

type ActionGroup =
    { Name: ResourceName
      Location: Location
      ShortName: string
      Tags: Map<string,string>
      EmailReceivers: EmailReceiver list }
    interface IArmResource with
        member this.ResourceId = resource.resourceId this.Name

        member this.JsonModel =
            {| resource.Create(this.Name, this.Location, tags = this.Tags) with
                   properties =
                       {| groupShortName = this.ShortName
                          enabled = true
                          emailReceivers =
                              [ for emailReceiver in this.EmailReceivers do
                                    {| name = emailReceiver.Name
                                       emailAddress = emailReceiver.EmailAddress
                                       useCommonAlertSchema = false |} ] |} |}
            :> _
