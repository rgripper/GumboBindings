#pragma once

#if defined(_WIN32) || defined(_WIN64) 
#define snprintf _snprintf 
#define vsnprintf _vsnprintf 
#define strcasecmp _stricmp 
#define strncasecmp _strnicmp 
#endif