# VakantiePark

## JWT-sleutel

De sleutel waarmee de API inlogtokens ondertekent staat niet in de repo.

- **Development:** zonder ingestelde sleutel maakt de API bij elke start een willekeurige sleutel aan. Na een herstart moet je opnieuw inloggen. Wil je dat niet, zet dan een vaste sleutel in user-secrets:
  `dotnet user-secrets set "JWT:Key" "<minimaal 32 willekeurige tekens>" --project VakantiePark.API`
- **Andere omgevingen:** zet de sleutel in de omgevingsvariabele `JWT__Key`. Zonder sleutel start de API niet.
