// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using Windows.Win32.Foundation;
using Windows.Win32.System.Com;

namespace System.Private.Windows.Ole;

/// <summary>
///  Simulates a native data object whose data disappears after a successful format query.
/// </summary>
internal unsafe class FailingGetDataNativeDataObject : NativeDataObjectMock
{
    private readonly ushort _format;
    private readonly TYMED _tymed;

    public FailingGetDataNativeDataObject(ushort format, TYMED tymed)
    {
        _format = format;
        _tymed = tymed;
    }

    public override HRESULT QueryGetData(FORMATETC* pformatetc)
    {
        if (pformatetc is null
            || pformatetc->cfFormat != _format
            || pformatetc->dwAspect != (uint)DVASPECT.DVASPECT_CONTENT
            || pformatetc->lindex != -1
            || pformatetc->tymed != (uint)_tymed)
        {
            return HRESULT.DV_E_FORMATETC;
        }

        return HRESULT.S_OK;
    }

    public override HRESULT GetData(FORMATETC* pformatetcIn, STGMEDIUM* pmedium)
        => HRESULT.CLIPBRD_E_CANT_OPEN;

    protected override void Dispose(bool disposing) { }
}
