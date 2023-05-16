
#ifndef TINTIN_COLOR_UTILITIES_INCLUDED
#define TINTIN_COLOR_UTILITIES_INCLUDED

void RGBtoCMYK_float(float3 rgb, out float4 cmyk)
{
    // https://www.eembc.org/techlit/datasheets/cmyk_consumer.pdf
    //#if defined(SHADERGRAPH_PREVIEW)
    //cmyk = float4(0.0, 0.0, 0.0, 0.0);
    //#else
    float k = 1.0 - max(rgb.r, max(rgb.g, rgb.b));
    float c = (1.0 - rgb.r - k) / (1.0 - k);
    float m = (1.0 - rgb.g - k) / (1.0 - k);
    float y = (1.0 - rgb.b - k) / (1.0 - k);
    cmyk = float4(saturate(c), saturate(m), saturate(y), saturate(k));
    
    /*float c = 1.0 - rgb.r;
    float m = 1.0 - rgb.g;
    float y = 1.0 - rgb.b;
    float k = min(c, min(m, y));
    cmyk = float4(c - k, m - k, y - k, k);*/
    //#endif
}

float4 RGBtoCMYK(float3 rgb)
{
    float c = 1.0 - rgb.r;
    float m = 1.0 - rgb.g;
    float y = 1.0 - rgb.b;
    float k = min(c, min(m, y));
    return float4(c - k, m - k, y - k, k);
}

#endif