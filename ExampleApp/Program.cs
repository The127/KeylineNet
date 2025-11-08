using KeylineClient.auth;

namespace ExampleApp;

internal static class Program
{
    public const string PrivateKeyPem =
        """
        -----BEGIN RSA PRIVATE KEY-----
        MIIEpAIBAAKCAQEA3xLc0aUHQznh8CDuKWmdu4yTosNtIC5/V840uuN9YooLkADn
        NwekFgKCXcrA1g+JQ6ADcMnraXOLUlkX0of5/ZgWtQUGzIWEHJlVTS7Faef7590w
        feK6kphggCru4ZSEez2iXJWZJ0hTFO0CbJgUSvGwKLtoq/6RK8DyRseBiJzZLmmC
        mQ/pY2WNO5cvGSjDXxV4qBXqXpYEJEcwjTxFwlqEB1hm6hZcHVrW7iLPY6y30Ydz
        HHrOTO6KqJY4hqdbfXqfma870CkOwpCDwY+3UKPfW8DPHi+j9D772EBEEjXB797a
        uXnQTA0vZOV08KLayR/M+guGeLUXdXRbyXGrfwIDAQABAoIBAD7Y8Z2fAQzPofgl
        TvQb7XSJho60wGnwr6/tSK3eqdS5T8iieIHohTIuQsxp8ypb9jA3RMDiIpDzHK12
        rF+EL8pirwMRB3lXpIDqMj4sjzHnSfFpP+A3oJjslUOG1QZ48VpHYxbyjtOibMXY
        gHZX4l+6+AaBxluuvPe2xItsafIyt58eyH3Z2vJLH3p6wAGXey2mIDk8wIIPKUyG
        K6N6qhsFY0T5AK/UD5bRKdcXX+aB7l7/3bK9nUkHZWnN1d20TxFleIyrNEiXr0tu
        LIUsGsHfroTc1G4LZd/cBw0DHXztI4Rp8ar7lUPpEVgdZFmFAwt4OmPQG0tUEGeS
        kMv59gECgYEA/Nc/DbuTGWM+pCX8PWTEy0c9jWaY9HxvYHznbKYbULKlVdXw3w3b
        IXe8teUaxoMVariUDpcRyLdA6xe1nM8RZH/kwJL9QenWgkfyH9Ik4BRjOpblSHWf
        MUpWLPQz9i0hZEWryouSk9btf39XvI/SiTLiIVX31fnK6ciRDLs2VocCgYEA4dxm
        sM7QUrdOJq1r/bwNDcMVZbf9tnRYnfIj+ldKeWtVXtp+94sx64Wpp8w7Lp51St2+
        pCb4/mTPJG4BAEVro4AvPpu3uxExYrr8j5c6ezN0xRgcZTtsO++KNbKoxpLaPQiD
        8wzjPVoVMBolgX0UP5yEk7zG8jkLg11t10ilyUkCgYEAp1QcGo6Er0yK7D9nS4og
        4xbmmWnI2CRx4T1IOxNDCIjx+nh3zGZzGxcPAKH6sl2WEubMtUstLdR5Vhx+yKQR
        Mp2hWgDtMm361IWgq4Z7eQCFGwU6AFY3YHt9xIpoyQbdDms9YfI0szqOOs6f1d6o
        yruuA7nNJwMFUuq1c+OB9okCgYEA4Zaz8tD+fj+cDUGV5T7YgvBhBNH61Svr/wYF
        LVvWhOPRxwXRXwpmd+lvBKwWSH/4gyhr66UJeX3S7333/f9YfVvg1FXP80Y+33AI
        JptTzA4fCWUFp47skgi1MOEbdgrjc6Z3tGEg7vx0wTC8WVNG0CpSuQUKaJDVkQXW
        WfqzrBECgYA9ifhbRKmQ/S9i73xVHvkRuJyTMakwhClxAdX/ukUrbYC043Aihj2C
        GibQ+w28liYyUb3frY42ZL40IFOkQmdPfurPQ/0T4q2bPUcXmKWrmBmjhFYvCAJe
        hAhjn2NWNtrt/W4ioC8UhFQ9bUQ6T+Mi2cPaWMGMN3xLkdt3bMArzw==
        -----END RSA PRIVATE KEY-----
        """;

    public const string PublicKeyPem =
        """
        -----BEGIN PUBLIC KEY-----
        MIIBIjANBgkqhkiG9w0BAQEFAAOCAQ8AMIIBCgKCAQEA3xLc0aUHQznh8CDuKWmd
        u4yTosNtIC5/V840uuN9YooLkADnNwekFgKCXcrA1g+JQ6ADcMnraXOLUlkX0of5
        /ZgWtQUGzIWEHJlVTS7Faef7590wfeK6kphggCru4ZSEez2iXJWZJ0hTFO0CbJgU
        SvGwKLtoq/6RK8DyRseBiJzZLmmCmQ/pY2WNO5cvGSjDXxV4qBXqXpYEJEcwjTxF
        wlqEB1hm6hZcHVrW7iLPY6y30YdzHHrOTO6KqJY4hqdbfXqfma870CkOwpCDwY+3
        UKPfW8DPHi+j9D772EBEEjXB797auXnQTA0vZOV08KLayR/M+guGeLUXdXRbyXGr
        fwIDAQAB
        -----END PUBLIC KEY-----
        """;

    private static async Task Main()
    {
        var authenticator = new ServiceUserAuthenticator(
            "http://localhost:8081/oidc/keyline/token",
            PrivateKeyPem,
            "test-service-user",
            "9ca0ce76-7f8d-4675-8af7-3b34b04ac976",
            "admin-ui"
        );
        var authHandler = new AuthenticationHandler(authenticator)
        {
            InnerHandler = new HttpClientHandler(),
        };

        var httpClient = new HttpClient(authHandler);
        httpClient.BaseAddress = new Uri("http://localhost:8081");

        var client = new KeylineClient.KeylineClient(httpClient, "keyline");

        var pagedResponse = await client.Projects["system"].Applications.List();
        pagedResponse.Items.ForEach(x => Console.WriteLine(x.Name));
    }
}