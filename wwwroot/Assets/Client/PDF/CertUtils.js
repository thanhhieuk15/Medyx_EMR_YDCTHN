// bien
var iCheck = checkBrowser();
var base64Cert = "MIIEDTCCAvWgAwIBAgIQVAMi4JmRLdP9rncYcjiAajANBgkqhkiG9w0BAQUFADBJMQswCQYDVQQGEwJWTjEOMAwGA1UEBxMFSGFub2kxGTAXBgNVBAoTEEJrYXYgQ29ycG9yYXRpb24xDzANBgNVBAMTBkJrYXZDQTAeFw0xNDAzMTMwOTE2NTlaFw0xNTAzMTMwOTE2NTlaMGkxHjAcBgoJkiaJk/IsZAEBDA5DTU5EOjAxMzU4MTUzMDEPMA0GA1UEAwwGSGFpTk1kMRUwEwYDVQQHDAxD4bqndSBHaeG6pXkxEjAQBgNVBAgMCUjDoCBO4buZaTELMAkGA1UEBhMCVk4wgZ8wDQYJKoZIhvcNAQEBBQADgY0AMIGJAoGBAMR4iWX4+AErp5laY4DWq9FAl5WrhRTatiXtOqpWlgZqvffMIM0h+yBKJrxXLpIfsfniVKhfPl6QupN4Zs7ELKNAHzuR01rI1igkXnPZU1h3iF69PNl8Z9cxpI+sKEvFhd1UqBwHuA8PAvQkPiwK2JzoBCbLG5cex0ZreVYarF7lAgMBAAGjggFTMIIBTzAxBggrBgEFBQcBAQQlMCMwIQYIKwYBBQUHMAGGFWh0dHA6Ly9vY3NwLmJrYXZjYS52bjAdBgNVHQ4EFgQUDi7Qua3Leh1tpRuHT7ssZ8r0j3swDAYDVR0TAQH/BAIwADAfBgNVHSMEGDAWgBQ96COZf0jtiOd5t6dVPCuwzAOo3TB/BgNVHR8EeDB2MHSgI6Ahhh9odHRwOi8vY3JsLmJrYXZjYS52bi9Ca2F2Q0EuY3Jsok2kSzBJMQ8wDQYDVQQDDAZCa2F2Q0ExGTAXBgNVBAoMEEJrYXYgQ29ycG9yYXRpb24xDjAMBgNVBAcMBUhhbm9pMQswCQYDVQQGEwJWTjAOBgNVHQ8BAf8EBAMCB4AwHwYDVR0lBBgwFgYIKwYBBQUHAwQGCisGAQQBgjcKAwwwGgYDVR0RBBMwEYEPaGFpbm1kQGJrYXYuY29tMA0GCSqGSIb3DQEBBQUAA4IBAQA2WKfLsDUSTjKL0MlziUi37kcgN7zAvEDuXvAhlhMGuvwc1wKJXanDWRAt/C2uNdlNUI2VTDRGMPgo1hvLAPB601/xxIW50CDsstRQXb3pq8n/HdxzU0ieqH4GxOJLUCoL6RUsiJ/n/1qWBHOoLbHhC2boVMxu00l/Ekm8/M5r4jHDlJP0wIN5ptuaRr5A4R9sXkL3QRJZO3ufi+J71PYAyVn4fcsfQZqPe9i4/wdRlqeQ59Zu3ytSdC2p6N5m2wkrl96vE4PW01eZqIzUxVUfAU8igftmiOnn9qMSdqsBRI1vKDrDgqX98vR+DvGavuC1DpPTOyxfau+UadMUJ6d/";
var timeCheck = new Date();//"2015/12/22 10:11:11";
var licenseKey = "PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiIHN0YW5kYWxvbmU9Im5vIj8+PExpY2Vuc2UgaWQ9IkwwMDEiPjxUZW5QaGFuTWVtPkJrYXZDQSBTaWduZXIgUGx1Z2luPC9UZW5QaGFuTWVtPjxOZ3VvaUNhcD5Ca2F2IENvcnBvcmF0aW9uPC9OZ3VvaUNhcD48RG9uVmlEdW9jQ2FwPlZTRDwvRG9uVmlEdW9jQ2FwPjxIYW5TdUR1bmc+PE5nYXlDYXA+MzAtMDMtMjAxNzwvTmdheUNhcD48TmdheUhldEhhbj4zMC0wOC0yMDE3PC9OZ2F5SGV0SGFuPjwvSGFuU3VEdW5nPjxRdXllblN1RHVuZz48VW5nRHVuZz4qPC9VbmdEdW5nPjxNb2R1bGVYTUw+MTwvTW9kdWxlWE1MPjxNb2R1bGVQREY+MTwvTW9kdWxlUERGPjxNb2R1bGVPT1hNTD4xPC9Nb2R1bGVPT1hNTD48TW9kdWxlQ01TPjE8L01vZHVsZUNNUz48TW9kdWxlQ2VydGlmaWNhdGVVdGlscz4xPC9Nb2R1bGVDZXJ0aWZpY2F0ZVV0aWxzPjwvUXV5ZW5TdUR1bmc+PFNpZ25hdHVyZSB4bWxucz0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnIyI+PFNpZ25lZEluZm8+PENhbm9uaWNhbGl6YXRpb25NZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy9UUi8yMDAxL1JFQy14bWwtYzE0bi0yMDAxMDMxNSNXaXRoQ29tbWVudHMiLz48U2lnbmF0dXJlTWV0aG9kIEFsZ29yaXRobT0iaHR0cDovL3d3dy53My5vcmcvMjAwMC8wOS94bWxkc2lnI3JzYS1zaGExIi8+PFJlZmVyZW5jZSBVUkk9IiI+PFRyYW5zZm9ybXM+PFRyYW5zZm9ybSBBbGdvcml0aG09Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvMDkveG1sZHNpZyNlbnZlbG9wZWQtc2lnbmF0dXJlIi8+PC9UcmFuc2Zvcm1zPjxEaWdlc3RNZXRob2QgQWxnb3JpdGhtPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwLzA5L3htbGRzaWcjc2hhMSIvPjxEaWdlc3RWYWx1ZT5SN0xJODJTOGJQYUQ3U2o0em1ONXZyUndYdTQ9PC9EaWdlc3RWYWx1ZT48L1JlZmVyZW5jZT48L1NpZ25lZEluZm8+PFNpZ25hdHVyZVZhbHVlPk9IM3JZZzIxcWNJWWxIS2ZiZFBTdjZzRFZuenBNZXRsejEvY1pIdG1qZldqWlNZd3V0NjRwelAzREJUL1VsTHdkMkJzY1M4SXVTNUkNCkdKR1ErM3d3T3hKbGMxWG9tMjJTQ0FKMFlXUTMyQ1BBa3kxUGozT1c4a2d3SXN4OFQ3QUVvZXJDTkxKS2w0NCtlVE15dWZOS1dZWjENCi9GWnluQ09YWUFSaGRuUmRJMG9MQys2VXpPODduRGFhMjgyL2tWOFMrUzdCQXBiNFRmckJUU2NCOWdzdlp6UTBkWFFnSW4vaUs5aTYNCitJWkdSSzJXOEtSZVRZZDlRSnYyR0RCbm02ejUzMDlSYWlJTWsra04zSENpYkJxQjljcGx4cG1JVXlmakROTThsbFE5cDRjMXh1U24NCm1mc3pBREdJMkVsT3ZFUXhtaU1ja2FpZXdBdWNhNUFIZUdSMk5nPT08L1NpZ25hdHVyZVZhbHVlPjxLZXlJbmZvPjxYNTA5RGF0YT48WDUwOVN1YmplY3ROYW1lPkM9Vk4sU1Q9SMOgIE7hu5lpLEw9Q+G6p3UgR2nhuqV5LE89Q8O0bmcgdHkgQ+G7lSBwaOG6p24gQmthdixPVT1CYW4gQU5NLENOPUJrYXZDQSBMaWNlbnNlLFVJRD1NU1Q6MDEwMTM2MDY5Ny1Ca2F2Q0FMaWNlbnNlPC9YNTA5U3ViamVjdE5hbWU+PFg1MDlDZXJ0aWZpY2F0ZT5NSUlFekRDQ0E3U2dBd0lCQWdJUVZBTkRQQmsvQ1pwbTNwTmRJQll1aFRBTkJna3Foa2lHOXcwQkFRVUZBREJKTVFzd0NRWURWUVFHDQpFd0pXVGpFT01Bd0dBMVVFQnhNRlNHRnViMmt4R1RBWEJnTlZCQW9URUVKcllYWWdRMjl5Y0c5eVlYUnBiMjR4RHpBTkJnTlZCQU1UDQpCa0pyWVhaRFFUQWVGdzB4TmpBMU16QXdOREF4TURSYUZ3MHhPREEzTWprd05EQXhNRFJhTUlHMU1Td3dLZ1lLQ1pJbWlaUHlMR1FCDQpBUXdjVFZOVU9qQXhNREV6TmpBMk9UY3RRbXRoZGtOQlRHbGpaVzV6WlRFWE1CVUdBMVVFQXd3T1FtdGhka05CSUV4cFkyVnVjMlV4DQpFREFPQmdOVkJBc01CMEpoYmlCQlRrMHhJakFnQmdOVkJBb01HVVBEdEc1bklIUjVJRVBodTVVZ2NHamh1cWR1SUVKcllYWXhGVEFUDQpCZ05WQkFjTURFUGh1cWQxSUVkcDRicWxlVEVTTUJBR0ExVUVDQXdKU01PZ0lFN2h1NWxwTVFzd0NRWURWUVFHRXdKV1RqQ0NBU0l3DQpEUVlKS29aSWh2Y05BUUVCQlFBRGdnRVBBRENDQVFvQ2dnRUJBSURuUTBBR3lnZ2gzTHpISXFwcUE4TFJOL2NwTURrV1FJbFA2YjhyDQpLQy9BbkdxNGJ4YXdORm5GV3o0NndpN1NWb0xicjY1Q0tlZjhDSjBJS0JsS0RVWWZjVHVVWTYxNEQ0NGtxMjRvWk1Vb01meTBDdm1lDQp3aU84TDV3Snh5TXNJTTNONVAxQVVwRU9OR0Q3UDdkbmJTNEFDcHpEdmRwMWUwZlI5dVNuNXhJNzVtOExUZzBEbTRVb0NZS0JaTlNXDQpqOUg0Tm84V240RjFXQ3lRb3FYY0IrVFJmVVZSLzdVcGY0YW9zOTdDVVZ2ZEd3V2xVdXlCeEcrcU1YaTdhZlV3bVlWTW9qVFhsUVVhDQp4QUxUQUpqblYvL2F0a3NoQy96NzNiZGh2WHo0cDA1YzBDb3BUNlBMYXRsRGx3RUUxcDU0Vk9mUGh3aE1VWDM5ZFByYzh6cStzMHNDDQpBd0VBQWFPQ0FVRXdnZ0U5TURFR0NDc0dBUVVGQndFQkJDVXdJekFoQmdnckJnRUZCUWN3QVlZVmFIUjBjRG92TDI5amMzQXVZbXRoDQpkbU5oTG5adU1CMEdBMVVkRGdRV0JCUWpGWTBPdmhXamtZY0Q3bm4yekF3NkFtdnU0VEFNQmdOVkhSTUJBZjhFQWpBQU1COEdBMVVkDQpJd1FZTUJhQUZCNndEMGlYMzlERFo2ZEdoRHRZTzRnTlU1U0dNSDhHQTFVZEh3UjRNSFl3ZEtBam9DR0dIMmgwZEhBNkx5OWpjbXd1DQpZbXRoZG1OaExuWnVMMEpyWVhaRFFTNWpjbXlpVGFSTE1Fa3hEekFOQmdOVkJBTU1Ca0pyWVhaRFFURVpNQmNHQTFVRUNnd1FRbXRoDQpkaUJEYjNKd2IzSmhkR2x2YmpFT01Bd0dBMVVFQnd3RlNHRnViMmt4Q3pBSkJnTlZCQVlUQWxaT01BNEdBMVVkRHdFQi93UUVBd0lGDQpvREFwQmdOVkhTVUVJakFnQmdnckJnRUZCUWNEQVFZSUt3WUJCUVVIQXdJR0Npc0dBUVFCZ2pjS0F3d3dEUVlKS29aSWh2Y05BUUVGDQpCUUFEZ2dFQkFDL0ZIMzV0TzBkeUFVdnBwQkJpZEU5c1RjYS8yRkwreUJQMWRQVWdwdEpUYzBmYmxqZTZxL0dqcXhJOFhyRTUremJmDQppUElROSt3dUhXb1VwUFpzdGQxSWhOUmNCZldvYm8rV3hEM0thWGJVZS9KRkNRalNoalVOczMxY2tBUGkxMlJISzdSV3NrOGx2bkdYDQo3QWNSK014bm5kRUdmOG1kMlhyWXdTd3dwYmpidXRHUnh3djJlRUwvNlZNRlhtQmM4ZlBLK0RpbjI0YVNWV09NOVdOU21aa1gxYkJ6DQprSVZMN2ViWkp3WnhUMElKVmJheDdkZ0U1V2hNb2xrellWNHl1K2xsU3RoQWtucENOQzhNVXFzb2REc0htaUZYc1M5bDJQZkRhZ1g0DQpIMnpKQkhJSEp0bkcrNEFQWHpHKzZZK2EyZ2grMytFYlpMNGxwRzY1a0FCclVtcz08L1g1MDlDZXJ0aWZpY2F0ZT48WDUwOUNlcnRpZmljYXRlPk1JSUVOekNDQXgrZ0F3SUJBZ0lLWVFyeFZBQUFBQUFBRHpBTkJna3Foa2lHOXcwQkFRVUZBREIrTVFzd0NRWURWUVFHRXdKV1RqRXoNCk1ERUdBMVVFQ2hNcVRXbHVhWE4wY25rZ2IyWWdTVzVtYjNKdFlYUnBiMjRnWVc1a0lFTnZiVzExYm1sallYUnBiMjV6TVJzd0dRWUQNClZRUUxFeEpPWVhScGIyNWhiQ0JEUVNCRFpXNTBaWEl4SFRBYkJnTlZCQU1URkUxSlF5Qk9ZWFJwYjI1aGJDQlNiMjkwSUVOQk1CNFgNCkRURTFNRFV4TlRBek1qVTBPRm9YRFRJd01EVXhOVEF6TXpVME9Gb3dTVEVMTUFrR0ExVUVCaE1DVms0eERqQU1CZ05WQkFjVEJVaGgNCmJtOXBNUmt3RndZRFZRUUtFeEJDYTJGMklFTnZjbkJ2Y21GMGFXOXVNUTh3RFFZRFZRUURFd1pDYTJGMlEwRXdnZ0VpTUEwR0NTcUcNClNJYjNEUUVCQVFVQUE0SUJEd0F3Z2dFS0FvSUJBUUREall5MkJ6bzVyMzN2bHdZVHA3cXhXeDRkcGZwaXphZjZlUTZ4ekVEUGVSUU4NCmptbVc2L1JGZzNkMXR2OGtjU1dXeDZLaHVtSUx6elpkdmZFUlhNa1FwVEd1ZXFxMzV6RzdkOUdVbGttSVZEeWNRNFZ3dm94cTlNVFcNCk5uclFzWW4vQVJxaXgxdUUwWk9zWW55YzNjY1NpMHJLWmtnT3J5QkdYVWRrbXV6SE8xWE1rOElHTjJBTGhnMElyMGxZK0RkQ200dGUNCmVhc2Iwc1lkY2JVRHdISlB0ZzFrRUplMVQybVhTd1lDTkhCdjdMZzdpbkMrQVJmeG9DMEFsYUhhWlVQekhIQm1XdHlKUjJXSHd1WXANCkRNRTRSbThOSnUwb2ZHN0IrTm5aZ3ExczJhR0tYbTRjeDRFOTV4UEpudkszZTZ3bmp4YUE1LzNYQ1phL0dXUWxCUkkxQWdNQkFBR2oNCmdlc3dnZWd3RWdZRFZSMFRBUUgvQkFnd0JnRUIvd0lCQURBTEJnTlZIUThFQkFNQ0FZWXdIUVlEVlIwT0JCWUVGQjZ3RDBpWDM5REQNClo2ZEdoRHRZTzRnTlU1U0dNQjhHQTFVZEl3UVlNQmFBRk0xaWNlUmh2ZjQ5N0xKQVlOT0JkZDA2ckd2R01Ed0dBMVVkSHdRMU1ETXcNCk1hQXZvQzJHSzJoMGRIQTZMeTl3ZFdKc2FXTXVjbTl2ZEdOaExtZHZkaTUyYmk5amNtd3ZiV2xqYm5KallTNWpjbXd3UndZSUt3WUINCkJRVUhBUUVFT3pBNU1EY0dDQ3NHQVFVRkJ6QUNoaXRvZEhSd09pOHZjSFZpYkdsakxuSnZiM1JqWVM1bmIzWXVkbTR2WTNKMEwyMXANClkyNXlZMkV1WTNKME1BMEdDU3FHU0liM0RRRUJCUVVBQTRJQkFRQlVlVW11citrYjhwWlVvdmlwU2JySFRUTjFYaXpRSVhha2w0b1gNClpWRk9wemFUTm5TUVdOTkFvNk1jVU5GMk5MMXE0eEdlRmNxYm53YzFkWUdqYWpyeDBTZmFLbzJGa2JoNjU2V2J4R1QxbXFFL3V3aisNCnhzK3o5ZGdjckwrelNTVCtoR2thdWN0YUFKS0xaVFlBYlNILzRWMWVkZEM3ZTBiUElWbzVpbk5ZUml2UHkxTUl1dzRORkdoOXM4bWwNCm5zUDZiaVcwaHQ4ZXJuTXg1WUhuUWZ3ZE0rSythcnpkaUpETHpyTlBSZm83Z1N5NTBjM2tKamZVRll4YlNkZ1RVWENERFc3bjR4WTUNCm90UE44Qk5DQTJURGJiSHNtYkpYdjNSYTVCMnJlNzM0YjBGUG16ejFCYW5ZT2FtMk5BbzcreUg3NWNKaFlpZUpSNk5zd3NoZGtTekM8L1g1MDlDZXJ0aWZpY2F0ZT48WDUwOUNlcnRpZmljYXRlPk1JSUQxekNDQXIrZ0F3SUJBZ0lRRytSemloOCt3STlIbjZiUE5jV1lJakFOQmdrcWhraUc5dzBCQVFVRkFEQitNUXN3Q1FZRFZRUUcNCkV3SldUakV6TURFR0ExVUVDaE1xVFdsdWFYTjBjbmtnYjJZZ1NXNW1iM0p0WVhScGIyNGdZVzVrSUVOdmJXMTFibWxqWVhScGIyNXoNCk1Sc3dHUVlEVlFRTEV4Sk9ZWFJwYjI1aGJDQkRRU0JEWlc1MFpYSXhIVEFiQmdOVkJBTVRGRTFKUXlCT1lYUnBiMjVoYkNCU2IyOTANCklFTkJNQjRYRFRBNE1EVXhOakF4TVRJME9Wb1hEVFF3TURVeE5qQXhNakF6TWxvd2ZqRUxNQWtHQTFVRUJoTUNWazR4TXpBeEJnTlYNCkJBb1RLazFwYm1semRISjVJRzltSUVsdVptOXliV0YwYVc5dUlHRnVaQ0JEYjIxdGRXNXBZMkYwYVc5dWN6RWJNQmtHQTFVRUN4TVMNClRtRjBhVzl1WVd3Z1EwRWdRMlZ1ZEdWeU1SMHdHd1lEVlFRREV4Uk5TVU1nVG1GMGFXOXVZV3dnVW05dmRDQkRRVENDQVNJd0RRWUoNCktvWklodmNOQVFFQkJRQURnZ0VQQURDQ0FRb0NnZ0VCQUtFL1dWRU8vakQvWWR1V2VCU0wyME04TnI1aHI5eTFQMkFlMHcwQlFhMzQNCnlZcENqc2p0TW9aSHhmNjE5K3JXUkRjUUVzTklDRkZRdXVWWDZjNDF5WTRjY3dtRk0wemh1emlzanEyM0V3UXVab0ZYTGN6N0d2MHUNCm5JdjlDVUR3WUJlYmNVVnRmZVBiS3RLN210M3J6RjdrQU4vVmJEQ0ZtNzFYZnkzVUpOT0ErK0FvVWI2dzFtRUh6T1dnUitlUmJTK0gNCldPaTByY0d4UnJQY1doMDRDZG43dFNlWW5sNzg4ZlJJLytpaE8vOVFNOWttcTdLWllwM01lOGhTVFo1Y1FvdHZkSDc4bEJQZUN0THcNCnRXcjRsa3hRbk9ZaGpzSGxsd0ZPelord1FCbDhHMWx2WERnWm1qZmEwWUU1RmpMdmdhMndJV3NSbDhMQkNMMXZJMXdFRDlNQ0F3RUENCkFhTlJNRTh3Q3dZRFZSMFBCQVFEQWdHR01BOEdBMVVkRXdFQi93UUZNQU1CQWY4d0hRWURWUjBPQkJZRUZNMWljZVJodmY0OTdMSkENCllOT0JkZDA2ckd2R01CQUdDU3NHQVFRQmdqY1ZBUVFEQWdFQU1BMEdDU3FHU0liM0RRRUJCUVVBQTRJQkFRQk1uYzErSXlDQUhDalANCjhQSEozeEhLc21sVG8vSmZETE5sbkM5VTRSeFFLdUJWRjhRWHZxaVRVVWFxaHUwa1pDOVBFNDZ3dEJTY2ZFTytMVTVqVW16YjFuQVgNCldVZGJvbHF6eDVaNnRnMzFMUTNaWkRxdjBGUTYwUk5vdHZvNERnWHI0UHd3OTB5YlgrTHVaM3Y0WXVwMHIzSlVUTlQ2WG92czY3Z24NCmdTeVlqdmZLb0ZHV2M4WVhpZm4wVTVjL1Y4UGJWU2hKYzA5S055cG5oTVVUdnNiSjdnbEhZcitvc3VwODVWOGsyenU0ZERXdzRZV1ANCmlwZElqdWQ0WjRuTDVhUUM3RnRYb2JuSGxyZkI2ZVZkanBtbXB5V2FIYkRPMWp0ck0vSytTZUV0MW9lQnVYYXVwL3pOczhaMk1xOU4NClVGSnNMUTJ5dmRkUTVkTjFZNTlkelFxWjwvWDUwOUNlcnRpZmljYXRlPjwvWDUwOURhdGE+PC9LZXlJbmZvPjwvU2lnbmF0dXJlPjwvTGljZW5zZT4=";
var dllName = "BkavCA,BkavCAv2S";
timeCheck = getDateTime();
var iRet;
var urlOCSP = "";
// ObjectCert
var objectCert = new ObjCertificate()
//objectCert.Base64Signed = base64Cert;
objectCert.TimeCheck = timeCheck;
objectCert.OcspUrl = urlOCSP;

function CheckValidateResult(iRet) {
    var dataRet = "Lỗi không xác định";
    switch (iRet) {
        case 1:
            dataRet = "Good";
            alert("Good")
            break;
        case -1:
            dataRet = "Lỗi trong việc load thư viện";
            alert("Lỗi trong việc load thư viện");
            break;
        case 4:
            dataRet = "Không lấy được thông tin cert";
            alert("Không lấy được thông tin cert");
            break;
        case -5:
            dataRet = "Chứng thư hết hạn hoặc chưa đến hạn";
            alert("Chứng thư hết hạn hoặc chưa đến hạn");
            break;
        case -6:
            dataRet = "Chứng thư đã bị thu hồi";
            alert("Chứng thư đã bị thu hồi");
            break;
        case -7:
            dataRet = "Chứng thư không có quyền ký dữ liệu";
            alert("Chứng thư không có quyền ký dữ liệu");
            break;
        case -8:
            dataRet = "Kiểm tra trusted path thất bại";
            alert("Kiểm tra trusted path thất bại");
            break;
        case -12:
            dataRet = "Không thể kiểm tra trạng thái thu hồi của CTS";
            alert("Không thể kiểm tra trạng thái thu hồi của CTS");
            break;
        default:
            alert("Lỗi không xác định");
            break;
    }
    document.getElementById("checkCertRet").value = dataRet;
    document.getElementById("checkCertRet").innerHTML = dataRet;
}
function CheckOCSPResult(iRet) {
    var dataRet = "Lỗi không xác định";
    switch (iRet) {
        case 0:
            dataRet = "Good";
            alert("Good")
            break;
        case -1:
            dataRet = "Lỗi trong việc load thư viện";
            alert("Lỗi trong việc load thư viện");
            break;
        case 4:
            dataRet = "Không lấy được thông tin cert";
            alert("Không lấy được thông tin cert");
            break;
        case 1:
            dataRet = "Chứng thư đã bị thu hồi";
            alert("Chứng thư đã bị thu hồi");
            break;
        case 2:
            dataRet = "Không tìm thấy issuer cert";
            alert("Không tìm thấy issuer cert");
            break;
        case 3:
            dataRet = "Không tìm thấy url của ocsp server";
            alert("Không tìm thấy url của ocsp server");
            break;
        default:
            alert("Lỗi không xác định");
            break;
    }
    document.getElementById("checkCertRet").value = dataRet;
    document.getElementById("checkCertRet").innerHTML = dataRet;
}

// Validate Cert
document.addEventListener(EXTENSION_EVENT_NAME.VALIDATE_CERTIFICATE, function (data) {
    var result = document.getElementById('hrSignedData').value;
    var iRet = parseInt(result);
    CheckValidateResult(iRet);
});
function ValidateCertificate(serial, jsCallback) {
    objectCert.CertificateSerialNumber = serial;
    objectCert.FunctionCallback = jsCallback;  //  Kết quả ký sẽ được plugin tự động trả về paramater của hàm này 
    try {
        if (iCheck == 1) {
            BkavCAPlugin.ValidateCertificate(objectCert);
        }
        else {
            BkavPluginSigner.ValidateCertificate(objectCert);
        }
    } catch (e) {
        alert(e);
    }
}
// CheckOCSP
document.addEventListener(EXTENSION_EVENT_NAME.CHECK_OCSP, function (data) {
    var result = document.getElementById('hrSignedData').value;
    var iRet = parseInt(result);
    CheckOCSPResult(iRet);

});
function CheckOCSP(serial, jsCallback) {
    objectCert.CertificateSerialNumber = serial;
    objectCert.FunctionCallback = jsCallback;  //  Kết quả ký sẽ được plugin tự động trả về paramater của hàm này 
    try {
        if (iCheck == 1) {
            BkavCAPlugin.CheckOCSP(objectCert);
        }
        else {
            BkavPluginSigner.CheckOCSP(objectCert);
        }
    } catch (e) {
        alert(e);
    }
}
// CheckValidTime
function CheckValidTime(serial, jsCallback) {
    objectCert.CertificateSerialNumber = serial;
    var iCheckCallback = document.getElementById('setCallback').value;
    if (iCheckCallback == 'yes') {
        objectCert.FunctionCallback = jsCallback;  //  Kết quả ký sẽ được plugin tự động trả về paramater của hàm này 
    }
    try {
        if (iCheck == 1) {
            BkavCAPlugin.CheckValidTime(objectCert);
        }
        else {
            var iRet = BkavPluginSigner.CheckValidTime(objectCert);
            if (iCheckCallback != 'yes') {
                alert(iRet);
            }
        }
    } catch (e) {
        alert(e);
    }
}
document.addEventListener(EXTENSION_EVENT_NAME.CHECK_VALID_TIME, function (data) {
    var result = document.getElementById('hrSignedData').value;
    console.log(result);
    alert(result);
});
// CheckCRL
function CheckCRL(serial, jsCallback) {
    objectCert.CertificateSerialNumber = serial;
    var iCheckCallback = document.getElementById('setCallback').value;
    if (iCheckCallback == 'yes') {
        objectCert.FunctionCallback = jsCallback;  //  Kết quả ký sẽ được plugin tự động trả về paramater của hàm này 
    }
    try {
        if (iCheck == 1) {
            BkavCAPlugin.CheckCRL(objectCert);
        }
        else {
            var iRet = BkavPluginSigner.CheckCRL(objectCert);
            if (iCheckCallback == 'yes') {
                alert(iRet);
            }
        }
    } catch (e) {
        alert(e);
    }
}

document.addEventListener(EXTENSION_EVENT_NAME.CHECK_CRL, function (data) {
    var result = document.getElementById('hrSignedData').value;
    console.log(result);
    alert(result);
});