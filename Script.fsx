#load ".\src\Domain\Domain.fs"

open Play14.Domain.Contacts

"cpontet" |> Twitter.create
"@cpontet" |> Twitter.create
"https://twitter.com/cpontet" |> Twitter.create

"cedric-pontet" |> LinkedIn.create
"/in/cedric-pontet" |> LinkedIn.create

"cedric@play14.org" |> EmailAddress.create
"cedric-play14.org" |> EmailAddress.create

open Play14.Domain.Sponsors

let name = SponsorName "My sponsor"
let website = "https://mysponsor.org" |> WebsiteUri.create
let logo = "https://mysponsor.org/logo.png" |> LogoUri.create
let email = "info@mysponsor.org" |> Contact.createEmail
let twitter = "mysponsor" |> Contact.createTwitter
let contacts = [ email ; twitter ]
let sponsor = {
    Name = name
    Website = website
    Logo = logo
    Contacts = contacts
}
