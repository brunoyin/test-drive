<#
    .Description
        Testing System.Net.Http.HttpClient
        https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler.-ctor?view=netframework-4.8

        Using a Python/Flask local http server that dumps request headers and data, I will test:
        GET
        POST Form
        PUT JSON
        PUT file upload

        1. start local Pyton/Flask http server: python ./dump_request.py
        2. 
        
    .Notes
        [System.Net.Http.HttpClient]::new
        [System.Net.Http.Headers.MediaTypeWithQualityHeaderValue]::new
        [System.Net.Http.HttpRequestMessage]::new
        [System.Net.Http.HttpClient]

        Add-Type -AssemblyName System.Net.Http

        $json_header = New-Object System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json")

        $client.DefaultRequestHeaders.Accept.Clear()
        $client.DefaultRequestHeaders.Accept.Add($json_header)
        $myip = 'https://api.ipify.org?format=json'

        $response = $client.GetAsync($myip).Result

        $json_content = New-Object System.Net.Http.StringContent("{A: 25, B:45}", 
            [System.Text.Encoding]::UTF8, "application/json")

#>

function Invoke-HttpRequest {
    <#
        .Description
            Send a HttpRequestMessage and process result
            Caller defines http headers and content of the request
        .Example
            # Get with authorization header, accept header
            $url = "http://127.0.0.1:5000/log"
            $method = [System.Net.Http.HttpMethod]::Get
            $req = [System.Net.Http.HttpRequestMessage]::new($method, $url)
            $ret = Invoke-HttpRequest -req $req
        .Example
            # FORM
            # Post with authorization header, accept header
            $url = "http://127.0.0.1:5000/log"
            $method = [System.Net.Http.HttpMethod]::Post
            $req = [System.Net.Http.HttpRequestMessage]::new($method, $url)
            [System.Collections.Generic.KeyValuePair[string,string][]]$formFields = @(
                [System.Collections.Generic.KeyValuePair[string,string]]::new("task", "Ask why") 
            )
            $req.Content = [System.Net.Http.FormUrlEncodedContent]::new($formFields)
            $ret = Invoke-HttpRequest -req $req
        .Example
            # JSON
            # Put with authorization header, accept header
            $url = "http://127.0.0.1:5000/log"
            $method = [System.Net.Http.HttpMethod]::Put
            $req = [System.Net.Http.HttpRequestMessage]::new($method, $url)
            $req.Headers.Authorization = [System.Net.Http.Headers.AuthenticationHeaderValue]::new('Basic', 
                [System.Convert]::ToBase64String([System.Text.ASCIIEncoding]::ASCII.GetBytes("bruno:yes")))
            $json_content = @{task = "Ask Why"} |ConvertTo-Json
            $utf8 = [System.Text.Encoding]::UTF8
            $req.Content = [System.Net.Http.StringContent]::new($json_content,$utf8, "application/json" )
            $ret = Invoke-HttpRequest -req $req
            $ret.content
            $ret
        .Example
            # file upload
            # Put with authorization header, accept header
            $url = "http://127.0.0.1:5000/log"
            $method = [System.Net.Http.HttpMethod]::Put
            $req = [System.Net.Http.HttpRequestMessage]::new($method, $url)
            $req.Headers.Authorization = [System.Net.Http.Headers.AuthenticationHeaderValue]::new('Basic', 
                [System.Convert]::ToBase64String([System.Text.ASCIIEncoding]::ASCII.GetBytes("bruno:yes")))
            $content = [System.Net.Http.MultipartFormDataContent]::new()
            $content.Headers.ContentType.MediaType = "multipart/form-data"
            $afile = 'D:\bruno\projects\powershell\test.ps1'
            [System.IO.Stream]$fileStream = [System.IO.File]::OpenRead($afile)
            $content.Add([System.Net.Http.StreamContent]::new($fileStream), 'file', [System.IO.Path]::GetFileName($afile))
            $req.Content = $content
            $ret = Invoke-HttpRequest -req $req
            $ret.content
            $ret
            $fileStream.Close()

    #>
    [CmdletBinding()]
    param (
        [System.Net.Http.HttpRequestMessage]$req
    )
    
    begin {
        try {
            [void][System.Net.Http.HttpClient]
        }
        catch {
            Add-Type -AssemblyName System.Net.Http | Out-Null
        }
        $client = [System.Net.Http.HttpClient]::new()
        $result = @{success = $false; content = $null; exception = $null; StatusCode=0}
    }
    
    process {
        try{
            $response = $client.SendAsync($req).GetAwaiter().GetResult()
            $result['StatusCode'] = [int]$response.StatusCode
            $result['content'] = $response.Content.ReadAsStringAsync().Result
            # determine success: EnsureSuccessStatusCode will throw an exception if http status code not 20x
            $response.EnsureSuccessStatusCode() |Out-Null
            $result['success'] = $true
        }
        catch {
            $result['exception'] = $_
            $result['success'] = $false
        }
        return $result
    }
    
    end {
    }
}

$test_upload = {
    # PUT upload
    $url = "http://127.0.0.1:5000/log"
    $method = [System.Net.Http.HttpMethod]::Put
    $req = [System.Net.Http.HttpRequestMessage]::new($method, $url)
    $req.Headers.Authorization = [System.Net.Http.Headers.AuthenticationHeaderValue]::new('Basic', 
        [System.Convert]::ToBase64String([System.Text.ASCIIEncoding]::ASCII.GetBytes("bruno:yes")))
    $content = [System.Net.Http.MultipartFormDataContent]::new()
    $content.Headers.ContentType.MediaType = "multipart/form-data"
    # replace this line below with a real file on your computer
    $afile = 'D:\bruno\projects\powershell\test.ps1'
    [System.IO.Stream]$fileStream = [System.IO.File]::OpenRead($afile)
    $content.Add([System.Net.Http.StreamContent]::new($fileStream), 'file', [System.IO.Path]::GetFileName($afile))
    $req.Content = $content
    $ret = Invoke-HttpRequest -req $req
    $ret.content
    $ret
}
Invoke-Command -ScriptBlock $test_upload

$test_json_put = {
    # JSON
    # Put with authorization header, accept header
    $url = "http://127.0.0.1:5000/log"
    $method = [System.Net.Http.HttpMethod]::Put
    $req = [System.Net.Http.HttpRequestMessage]::new($method, $url)
    $req.Headers.Authorization = [System.Net.Http.Headers.AuthenticationHeaderValue]::new('Basic', 
        [System.Convert]::ToBase64String([System.Text.ASCIIEncoding]::ASCII.GetBytes("bruno:yes")))
    $json_content = @{task = "Ask Why"} |ConvertTo-Json
    $utf8 = [System.Text.Encoding]::UTF8
    $req.Content = [System.Net.Http.StringContent]::new($json_content,$utf8, "application/json" )
    $ret = Invoke-HttpRequest -req $req
    $ret.content
    $ret
    $fileStream.Close()
}
Invoke-Command -ScriptBlock $test_json_put


