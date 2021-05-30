module ActionGroup

open Expecto
open Farmer
open Farmer.Builders
open Farmer.Arm
open TestHelpers

let private asJson (arm: IArmResource) = arm.JsonModel |> convertTo<{| kind: string; properties: {| statisticsEnabled: bool |} |}>

let tests = testList "Action Group" [
    test "Basic test" {
        let name_ = "name"
        let shortName = "short"
        let emailReceivers = [] //{"name" "email"}
        let tags = [ "a", "1"; "b", "2" ]
        let ag = actionGroup {
            name name_
            shortname shortName
            add_tags tags
            emailreceivers  emailReceivers 
        }
        let baseArm = (ag :> IBuilder).BuildResources(Location.WestEurope).[0]
        let agArm = baseArm :?> ActionGroup.ActionGroup
        let jsonModel = asJson baseArm
        Expect.equal agArm.Name (ResourceName name_) "Name"
        Expect.equal agArm.Location Location.WestEurope "Location"
        Expect.equal agArm.ShortName shortName "Short name"
        Expect.equal agArm.EmailReceivers emailReceivers "Email receivers"
        Expect.equal agArm.Tags (Map tags) "Tags"
    }

    test "Default options test" {
        let name_ = "name"
        let ag = actionGroup {
            name name_
        }

        let baseArm = (ag :> IBuilder).BuildResources(Location.WestEurope).[0]
        let agArm = baseArm :?> ActionGroup.ActionGroup
        let jsonModel = asJson baseArm
        Expect.equal agArm.Name (ResourceName name_) "Name"
        Expect.equal agArm.Location Location.WestEurope "Location"
        Expect.equal agArm.ShortName "" "Short name"
        Expect.sequenceEqual agArm.EmailReceivers [] "Email receivers"
        Expect.isEmpty agArm.Tags "Tags"
    }
]