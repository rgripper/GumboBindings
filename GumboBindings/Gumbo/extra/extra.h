#pragma once

#include <crtdefs.h>
#include <../src/gumbo.h>

#ifdef __cplusplus
extern "C" {
#endif

void GumboOptions gumbo_get_default_options(GumboOptions* options)
{
	return kGumboDefaultOptions;
}

#ifdef __cplusplus
}
#endif