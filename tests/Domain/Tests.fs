namespace Play14.Domain.Tests

open System
open Xunit
open Swensen.Unquote

module Contacts =

    open Play14.Domain.Contacts

    [<Fact>]
    let ``Simple string is a valid Twitter contact`` () =
        let twitter = "cpontet" |> Twitter.create
        test <@ twitter.ToString() = "Twitter https://twitter.com/cpontet" @>
    
    [<Fact>]
    let ``Handle with at sign is a valid Twitter contact`` () =
        let twitter = "@cpontet" |> Twitter.create
        test <@ twitter.ToString() = "Twitter https://twitter.com/cpontet" @>

    [<Fact>]
    let ``Url of the twitter account is a valid Twitter contact`` () =
        let twitter = "https://twitter.com/cpontet" |> Twitter.create
        test <@ twitter.ToString() = "Twitter https://twitter.com/cpontet" @>
        
