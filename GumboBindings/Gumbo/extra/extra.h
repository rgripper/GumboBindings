#pragma once

#include <crtdefs.h>
#include <../src/gumbo.h>

#ifdef __cplusplus
extern "C" {
#endif

void gumbo_set_options_defaults(GumboOptions* options)
{
	options->allocator = kGumboDefaultOptions.allocator;
	options->deallocator = kGumboDefaultOptions.deallocator;
	options->userdata = NULL;
	options->tab_stop = kGumboDefaultOptions.tab_stop;
	options->stop_on_first_error = kGumboDefaultOptions.stop_on_first_error;
	options->userdata = kGumboDefaultOptions.userdata;
}

#ifdef __cplusplus
}
#endif