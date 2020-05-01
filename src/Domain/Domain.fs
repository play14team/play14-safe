namespace Play14.Domain

module Contacts =

    open System

    type Twitter = Twitter of Uri

    module Twitter =
        let create ( handle : string) : Twitter =
            let name = handle.Split('/', '@') |> Seq.last
            sprintf "https://twitter.com/%s" name |> Uri |> Twitter

    type Facebook = Facebook of Uri

    module Facebook =
        let create ( page : string ) : Facebook =
            let name = page.Split('/', '@') |> Seq.last
            sprintf "http://facebook.com/%s" name |> Uri |> Facebook

    type LinkedIn = LinkedIn of Uri

    module LinkedIn =
        let create ( account : string ) : LinkedIn =
            let name = account.Split('/', '@') |> Seq.last
            sprintf "https://www.linkedin.com/in/%s" name |> Uri |> LinkedIn

    type SocialMedia =
    | Twitter of Twitter
    | Facebook of Facebook
    | LinkedIn of LinkedIn

    type EmailAddress = EmailAddress of string

    module EmailAddress = 
        let create ( email : string ) : EmailAddress =
            let regex = Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*")
            if not (email |> regex.IsMatch) then
                raise <| ArgumentException("email", "Email is invalid")

            email |> EmailAddress

    type Contact =
    | Email of EmailAddress
    | Social of SocialMedia

    module Contact =
        let createEmail (email : string) : Contact =
            email |> EmailAddress.create |> Email

        let createTwitter (name : string) : Contact =
            name |> Twitter.create |> Twitter |> Social 
        
        let createFacebook (name : string) : Contact =
            name |> Facebook.create |> Facebook |> Social

        let createLinkedIn (name : string) : Contact =
            name |> LinkedIn.create |> LinkedIn |> Social


module Sponsors =

    open System
    open Contacts

    type SponsorName = SponsorName of string
    type WebsiteUri = WebsiteUri of Uri
    module WebsiteUri = 
        let create (uri : string) : WebsiteUri =
            uri |> Uri |> WebsiteUri

    type LogoUri = LogoUri of Uri
    module LogoUri = 
        let create (uri : string) : LogoUri =
            uri |> Uri |> LogoUri

    type Sponsor = {
        Name : SponsorName
        Website : WebsiteUri
        Logo : LogoUri
        Contacts : Contact seq
    }

module Players =

    open System
    open Contacts

    type FirstName = FirstName of string
    type LastName = LastName of string
    type PlayerName = {
        FirstName : FirstName
        LastName : LastName
    }

    type Bio = PlayerBio of string
    type Company = PlayerCompany of string
    type Avatar = Avatar of Uri
    type AttendedEvent = AttendedEvent of string

    type Player = {
        Name : PlayerName
        Bio : Bio
        Company : Company
        Avatar : Avatar
        Contacts : Contact seq
        Events : AttendedEvent seq
    }

    type Host = Host of Player
    type Mentor = Mentor of Host

    type Create = PlayerName -> Player
    type MakeHost = Player -> Host
    type MakeMentor = Host -> Mentor

module Events =

    open System
    open Players
    open Sponsors
    
    type Image = Uri
    type EventName = EventName of string
    type Schedule = EventSchedule
    type TimeTable = EventTimeTable
    
    type EventStatus =
        | Draft
        | Published
        | Candelled
        | Rescheduled
        | Ended
    

    type VenueName = VenueName of string
    type VenueArea = VenueArea of string
    type VenueAddress = VenueAddress of string
    type VenueWebsite = VenueWebsite of Uri
    type MapUri = MapUri of Uri
    type Venue = {
        Name : VenueName
        Area : VenueArea
        Address : VenueAddress
        Website : VenueWebsite
        Map : MapUri
    }

    type EventVenue =
        | ToBeDefined
        | Venue of Venue
    
    type LinkRegistration = LinkRegistration of Uri
    
    type EventbriteId = EventbriteId of string
    type EventbriteRegistration = {
        EventId : EventbriteId
    }
    
    type WeezeventId = WeezeventId of string
    type WeezeventCode = WeezeventCode of string
    type WeezeventRegistration = {
        EventId : WeezeventId
        Code : WeezeventCode
    }
    
    type Registration =
       | NotOpened
       | Link of LinkRegistration
       | Eventbrite of EventbriteRegistration
       | Weezevent of WeezeventRegistration
       | Closed
    
    type UnpubishedEvent = {
        Name : EventName
        Schedule : Schedule
        TimeTable : TimeTable
        Status : EventStatus
        Venue : EventVenue
        Images : Image seq
        TeamMembers : Host seq
        Mentors : Mentor seq
        Sponsors : Sponsor seq
    }

    type PublishedEvent = {
        Name : EventName
        Schedule : Schedule
        TimeTable : TimeTable
        Status : EventStatus
        Venue : EventVenue
        Images : Image seq
        TeamMembers : Host seq
        Mentors : Mentor seq
        Sponsors : Sponsor seq
        Registration : Registration
    }

    type CancelledEvent = CancelledEvent of PublishedEvent
    type PostponedEvent = PostponedEvent of PublishedEvent
    type EndedEvent = EndedEvent of PublishedEvent

    type Create = EventName -> UnpubishedEvent
    type Publish = UnpubishedEvent -> Registration -> PublishedEvent
    type Cancel = PublishedEvent -> CancelledEvent
    type Postpone = PublishedEvent -> PostponedEvent
    type Reschedule = PostponedEvent -> PublishedEvent
    type MarkEnded = PublishedEvent -> EndedEvent
