<Query Kind="Program">
  <NuGetReference>AngleSharp</NuGetReference>
  <Namespace>AngleSharp</Namespace>
  <Namespace>AngleSharp.Dom</Namespace>
  <Namespace>System.Threading.Tasks</Namespace>
</Query>


class Result
{
	public string EndPoint;
	public string RegionName;
	public bool IsGlobal;
}

async Task Main()
{

	//var cellSelector = "tr.vevent td:nth-child(3)";
	//var cells = document.QuerySelectorAll(cellSelector);
	//var titles = cells.Select(m => m.TextContent);
	//titles.Dump();


	// get service urls
	var config = Configuration.Default.WithDefaultLoader();
	//var address = "https://en.wikipedia.org/wiki/List_of_The_Big_Bang_Theory_episodes";
	var address = "https://docs.aws.amazon.com/ja_jp/general/latest/gr/aws-service-information.html";
	var context = BrowsingContext.New(config);
	var document = await context.OpenAsync(address);

	var cellSelector = "div.highlights ul li a";
	var cells = document.QuerySelectorAll(cellSelector);

	//foreach (IElement cell in cells) {
	//	var url = "https://docs.aws.amazon.com/ja_jp/general/latest/gr/" + cell.GetAttribute("Href").TrimStart('.','/');
	//	url.Dump();
	//}

	var service_urls = cells.Select(c => "https://docs.aws.amazon.com/ja_jp/general/latest/gr/" + c.GetAttribute("Href").TrimStart('.', '/')).ToArray();
	var service_doc = await context.OpenAsync(service_urls[0]);
	//var selector = "div.table-contents table tbody tr td:nth-child(2)";
	//			service_doc.Dump();

	var result = new Result();

	// us-east-1
	var region_selector = "#w428aab9b9b5b5 tbody tr td:nth-child(2)";
	var region = service_doc.QuerySelectorAll(region_selector);
	result.RegionName = region.Select(e => e.TextContent).ToArray()[0];

	// end-point
	var end_point_selector = "#w428aab9b9b5b5 tbody tr td:nth-child(3)";
	var end_point = service_doc.QuerySelectorAll(end_point_selector);
	//end_point.Dump();
	result.EndPoint = end_point.Select(e => e.TextContent).ToArray()[0];

	// 1サービスできた
	result.IsGlobal = result.EndPoint.IndexOf(result.RegionName) > 0;
	result.IsGlobal.Dump();
}

