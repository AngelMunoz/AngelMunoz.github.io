#load "../siteModel.fsx"
#load "default.fsx"

open System
open Html
open SiteModel

[<CLIMutable>]
type Model =
    { title: string
      published: System.DateTime
      author: string
      summary: string
      tags: string seq
      category: string seq }

let generate (siteModel: SiteModel) (mdl: Model) (posts: Post list) (content: string) =
    let post =
        article [ Class "post" ]
            [ h1 [ Class "title" ] [ !!mdl.title ]
              div [ Class "is-size-5" ]
                  [ yield !!mdl.published.ToShortDateString()
                    if mdl.published.Subtract(TimeSpan.FromDays(365.0)).Year < DateTime.Today.Year then
                        yield h3 [ Class "has-text-warning is-size-4" ]
                                  [ !!"Notice! This post seems to be at least 1 year old, the contents might not be accurate anymore." ] ]
              div []
                  [ ul [ Class "tags" ]
                        [ for tag in mdl.tags do
                            yield li [ Class "tag is-light" ] [ a [] [ !!tag ] ] ] ]
              section [ Class "content" ] [ !!content ]
              section [ Id "disqus_thread" ] [ script [ Src "/scripts/disqus.js" ] [] ] ]

    Default.defaultPage siteModel mdl.title [ post ]
