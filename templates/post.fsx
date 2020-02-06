#load "../siteModel.fsx"
#load "default.fsx"

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
            [ h1 [ Class "post-title" ] [ !!mdl.title ]
              div [ Class "post-date" ] [ !!mdl.published.ToShortDateString() ]
              div []
                  [ ul [ Class "tags" ]
                        [ for tag in mdl.tags do
                            yield li [ Class "tag is-light" ] [ a [] [ !!tag ] ] ] ]
              section [ Class "section"] [!!content]
              section [Id "disqus_thread"] [
                script [ Src "/scripts/disqus.js"] []
              ]
            ]

    Default.defaultPage siteModel mdl.title [ post ]
