﻿using System;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.OLE.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.TextManager.Interop;

namespace PasteHtmlAsClass
{
    internal class GeneratingClassCommandFilter : IOleCommandTarget
    {
        private readonly IOleCommandTarget _nextCommandTargetInChain;
        private readonly IWpfTextView _wpfTextView;

        public GeneratingClassCommandFilter(IVsTextView textView,
            IWpfTextView wpfTextView)
        {
            _wpfTextView = wpfTextView;
            textView.AddCommandFilter(this, out _nextCommandTargetInChain);
        }

        public int QueryStatus(ref Guid pguidCmdGroup, uint cCmds, OLECMD[] prgCmds, IntPtr pCmdText)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (pguidCmdGroup == GuidConstant.GeneratingJsonCommandId && cCmds == 1 && prgCmds[0].cmdID == 0x100)
            {
                AllowInsertion(x =>
                {
                    if (x)
                    {
                        prgCmds.SetVisibility();
                    }
                    else
                    {
                        prgCmds.SetInvisible();
                    }
                });

                return VSConstants.S_OK;
            }

            return _nextCommandTargetInChain.QueryStatus(ref pguidCmdGroup, cCmds, prgCmds, pCmdText);
        }

        public int Exec(ref Guid pguidCmdGroup, uint nCmdID, uint nCmdexecopt, IntPtr pvaIn, IntPtr pvaOut)
        {
            ThreadHelper.ThrowIfNotOnUIThread();

            if (pguidCmdGroup == GuidConstant.GeneratingJsonCommandId && nCmdID == 0x100)
            {
                AllowInsertion(x =>
                {
                    if (!x)
                    {
                        return;
                    }

                    var html = ClipboardSupport.GetClipboardData();
                    if (string.IsNullOrEmpty(html))
                    {
                        return;
                    }

                    // TODO
                });
            }

            return _nextCommandTargetInChain.Exec(ref pguidCmdGroup, nCmdID, nCmdexecopt, pvaIn, pvaOut);
        }

        private void AllowInsertion(Action<bool> action)
        {
            if (action == null)
            {
                return;
            }

            var snapshot = _wpfTextView.TextBuffer.CurrentSnapshot;
            var document = snapshot.GetOpenDocumentInCurrentContextWithChanges();
            if (document != null)
            {
                var syntaxTree = ThreadHelper.JoinableTaskFactory.Run(() => document.GetSyntaxTreeAsync());
                if (syntaxTree != null)
                {
                    var root = ThreadHelper.JoinableTaskFactory.Run(() => syntaxTree.GetRootAsync());
                    var node = root.FindNode(_wpfTextView.GetTextSpan());
                    action(node is NamespaceDeclarationSyntax || node is ClassDeclarationSyntax);
                }
            }
        }

        public static void Register(IVsTextView textView, IWpfTextView wpfTextView)
        {
            var _ = new GeneratingClassCommandFilter(textView, wpfTextView);
        }
    }
}