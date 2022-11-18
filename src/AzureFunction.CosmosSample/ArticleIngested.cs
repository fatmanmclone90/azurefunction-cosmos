using System;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace AzureFunction.CosmosSample
{
    public record ArticleIngested(
        [JsonProperty(PropertyName = "id")]
        Guid id,
        string RecordType,
        List<string> WcsProviders,
        string ArticleNumber,
        string ItemDescription,
        string SizeGroup,
        string OptionDescription,
        string ColourCode,
        string ColourName,
        string Division,
        string SubDivision,
        string YearSeasonPhase,
        string AdditionalText,
        string BonusActivity,
        string SizeFactor,
        List<string> ArticleAttributes,
        string Length,
        string Width,
        string Height,
        string ThicknessInPocket,
        string Weight,
        MetadataModel Metadata);

    public record MetadataModel(UpdateModel Update);

    public record UpdateModel(string Source, string LineNumber);
}