#if !FORNAX
#r "Facades/netstandard"
#endif
#r "./_bin/Fornax.Core.dll"

[<CLIMutable>]
type SiteModel =
    { title: string
      author: string
      url: string }
